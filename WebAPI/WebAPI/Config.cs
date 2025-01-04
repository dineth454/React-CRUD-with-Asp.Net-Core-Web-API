using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI
{
    public static class IdentityConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("role", "user_role", new[] { "role" }) // Add roles identity resource
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                //new ApiScope("api1", "ISAuthAPI")
                new ApiScope("api1")
                {
                    UserClaims = { "role" } // Include the role claim in API scopes
                }
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    //AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1", "role" },
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
    }

    public class CustomCorsPolicyService : ICorsPolicyService
    {
        private readonly HashSet<string> _allowedOrigins = new HashSet<string>
        {
            "http://localhost:3000" // Add your allowed origins here
        };

        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            return Task.FromResult(_allowedOrigins.Contains(origin));
        }
    }
}
