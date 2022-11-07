using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RandomUsers.App;
using RandomUsers.App.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IRandomUsersApiService, RandomUserApiService>(
    client => client.BaseAddress = new Uri("https://randomuser.me/"));

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
