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
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope("api1", "ISAuthAPI")
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
                    AllowedScopes = { "api1" }
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
