using Blazored.LocalStorage;
using HomePage.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using Roman015API.Clients;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HomePage
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

            Roman015APIClientSetup(builder);
            RadzenSetup(builder);
            BlazoredLocalStorageSetup(builder);

            await builder.Build().RunAsync();
        }

        public static void Roman015APIClientSetup(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration
                    .Bind("AzureAd", options.ProviderOptions.Authentication);

                options.ProviderOptions.DefaultAccessTokenScopes
                    .Add("api://f744ceb4-e6cf-4923-a971-c7fc4a600fe2/ApiScope");

                options.ProviderOptions.LoginMode = "redirect";

            });
            builder.Services.AddAuthorizationCore(config =>
            {
                config.AddPolicy("BlogAdministratorsOnly", policy => policy.AddRequirements(new PermittedRoleRequirement("BlogAdministrator")));
            });

            builder.Services.AddSingleton<TestSignalRHub>(sp => {
                return TestSignalRHub.GetSingletonInstance();
            });

            #region For FE RBAC
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermittedRolePolicyProvider>();
            builder.Services.AddSingleton<IAuthorizationHandler, PermittedRoleAuthorizationHandler>();
            #endregion

            builder.Services.AddScoped<BlogEditorAPIClient>();
        }

        public static void RadzenSetup(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();
        }

        public static void BlazoredLocalStorageSetup(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddBlazoredLocalStorage();
        }
    }
}
