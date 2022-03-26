using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RazorBlogEditor;
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
            Roman015APIClientSetup(builder);
            BlazoredLocalStorageSetup(builder);
            BlazoredToastSetup(builder);

            await builder.Build().RunAsync();
        }

        public static void Roman015APIClientSetup(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddSingleton<StarWarsHub>(sp => {
                return StarWarsHub.GetSingletonInstance();
            });            
        }

        public static void BlazoredLocalStorageSetup(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddBlazoredLocalStorage();
        }

        public static void BlazoredToastSetup(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddBlazoredToast();
        }
    }
}
