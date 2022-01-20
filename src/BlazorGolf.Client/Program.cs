using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using BlazorGolf.Client;
using BlazorGolf.Client.Authentication;
using BlazorGolf.Client.Services;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//var blazorGolfApiUri = new Uri(builder.Configuration.GetValue<string>("ApiEnpointUrl"));
var blazorGolfApiUri = builder.HostEnvironment.IsDevelopment() ?
                             new Uri(builder.Configuration.GetValue<string>("DevApiEndPointUrl")) :
                                                        new Uri(builder.Configuration.GetValue<string>("ApiEndPointUrl"));

void RegisterTypedClient<TClient, TImplementation>(Uri apiBaseUrl) where TClient : class where TImplementation : class, TClient
{
    builder.Services.AddHttpClient<TClient, TImplementation>(client =>
    {
        client.BaseAddress = apiBaseUrl;
    });
}
// Register typed HTTP services
RegisterTypedClient<ICourseService, CourseService>(blazorGolfApiUri);

builder.Services.AddMsalAuthentication<RemoteAuthenticationState, CustomUserAccount>(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://weather/weather.read");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("profile");
    options.UserOptions.RoleClaim = "appRole";
}).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount, CustomUserAccountFactory>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
