using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mango.IdentityServer;

public static class Statics
{
    public const string AdminRole = "Admin";
    public const string CustomerRole = "Customer";

    public const string MangoAdminScope = "MangoAdmin";

    public static IReadOnlyList<IdentityResource> IdentityResources
        = new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
        };

    public static IReadOnlyList<ApiScope> ApiScopes = new List<ApiScope>
    {
        new ApiScope(MangoAdminScope, "Mango Admin Scope")
    };

    public static IReadOnlyList<Client> GenerateClients(IConfiguration configuration) => new List<Client>
    {
        new Client
        {
            ClientId="MangoWeb",
            ClientSecrets = { new Secret(configuration["Identity:Web:Secret"].Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { configuration["Identity:Web:RedirectUri"] },
            PostLogoutRedirectUris = { configuration["Identity:Web:PostLogoutRedirectUri"] },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                MangoAdminScope
            }
        }
    };
}