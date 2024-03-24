using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Authentication;
using BookStoreApp.Blazor.WebAssembly.UI;
using BookStoreApp.Blazor.WebAssembly.UI.Services;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BookStoreApp.Blazor.WebAssembly.UI.Configurations;
using BookStoreApp.Blazor.WebAssembly.UI.Providers;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7076") });

builder.Services.AddBlazoredLocalStorage();
//builder.Services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri("https://localhost:7076"));

builder.Services.AddScoped<ApiAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<ApiAuthStateProvider>());

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IClient, Client>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();




builder.Services.AddAutoMapper(typeof(MapperConfig));

await builder.Build().RunAsync();
