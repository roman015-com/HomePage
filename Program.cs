using Blazored.LocalStorage;
using Blazored.Toast;
using HomePage.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Roman015API.Clients;
using System;
using System.Net.Http;
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

            builder.Services.AddScoped<LazyAssemblyLoader>();

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration
                    .Bind("AzureAd", options.ProviderOptions.Authentication);

                options.ProviderOptions.DefaultAccessTokenScopes
                    .Add("api://f744ceb4-e6cf-4923-a971-c7fc4a600fe2/ApiScope");

                options.ProviderOptions.LoginMode = "redirect";

            });

            builder.Services.AddBlogEditor();
            builder.Services.AddHomePageToys();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredToast();

            await builder.Build().RunAsync();
        }
    }

    public static class IServiceCollectionExtensions
    {
        public static void AddBlogEditor(this IServiceCollection ServiceCollection)
        {
            ServiceCollection.AddAuthorizationCore(config =>
            {
                config.AddPolicy("BlogAdministratorsOnly", policy => policy.AddRequirements(new PermittedRoleRequirement("BlogAdministrator")));
            });

            #region For FE RBAC
            ServiceCollection.AddSingleton<IAuthorizationPolicyProvider, PermittedRolePolicyProvider>();
            ServiceCollection.AddSingleton<IAuthorizationHandler, PermittedRoleAuthorizationHandler>();
            #endregion

            ServiceCollection.AddScoped<BlogEditorAPIClient>();
        }

        public static void AddHomePageToys(this IServiceCollection ServiceCollection)
        {
            ServiceCollection.AddSingleton<StarWarsHub>(sp => {
                return StarWarsHub.GetSingletonInstance();
            });
        }
    }

}
