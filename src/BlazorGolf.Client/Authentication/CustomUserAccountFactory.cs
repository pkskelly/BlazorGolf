using System.Text.Json.Serialization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace BlazorGolf.Client.Authentication
{
    public class CustomUserAccountFactory : AccountClaimsPrincipalFactory<CustomUserAccount>
    {
        private readonly ILogger<CustomUserAccountFactory> _logger;

        public CustomUserAccountFactory(NavigationManager navigationManager, IAccessTokenProviderAccessor accessor, ILogger<CustomUserAccountFactory> logger) : base(accessor)
        {
            _logger = logger;
        }

        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(CustomUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);
            if (initialUser.Identity.IsAuthenticated)
            {   
                var userIdentity = (ClaimsIdentity)initialUser.Identity;
                foreach (var role in account.Roles)
                {             
                    _logger.LogInformation($"Adding appRole claim: {role}");
                    userIdentity.AddClaim(new Claim("appRole", role));
                }
                foreach (var wid in account.Wids)
                {
                    _logger.LogInformation($"Adding directoryRole claim: {wid}");
                    userIdentity.AddClaim(new Claim("directoryRole", wid));
                }
            }
            return initialUser;
        }        
    }
}