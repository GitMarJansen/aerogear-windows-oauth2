using AeroGear.OAuth2;
using System.Threading.Tasks;

namespace aerogear_windows_oauth2_config
{
    public class KeycloakConfig : Config
    {
        public async static Task<KeycloakConfig> Create(string clientId, string host, string realm)
        {
            var protocol = await ManifestInfo.GetProtocol();
            var defaulRealmName = clientId + "-realm";
            var realmName = realm != null ? realm : defaulRealmName;
            return new KeycloakConfig()
            {
                baseURL = host + "/auth/",
                authzEndpoint = string.Format("realms/{0}/tokens/login", realmName),
                redirectURL = protocol + ":/oauth2Callback",
                accessTokenEndpoint = string.Format("realms/{0}/tokens/access/codes", realmName),
                clientId = clientId,
                refreshTokenEndpoint = string.Format("realms/{0}/tokens/refresh", realmName),
                revokeTokenEndpoint = string.Format("realms/%@/tokens/logout", realmName),
                accountId = clientId
            };
        }
    }
}
