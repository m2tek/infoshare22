using Infoshare22.WebApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Blazor",client=>client.BaseAddress= new Uri(builder.HostEnvironment.BaseAddress) );
builder.Services.AddHttpClient("Functions", client => client.BaseAddress = new Uri(builder.Configuration["functionsUrl"]!));
builder.Services.AddMudServices();

await builder.Build().RunAsync();
