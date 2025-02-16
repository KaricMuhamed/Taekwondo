using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TakwondoFrontend;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using TakwondoFrontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register the HTTP client with the base URL of the app
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7040") });

// Register the authentication service
builder.Services.AddScoped<AuthService>();

// No need to add IJSRuntime manually
// It is already provided by Blazor WebAssembly.

await builder.Build().RunAsync();
