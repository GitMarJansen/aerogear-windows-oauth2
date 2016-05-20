using AeroGear.OAuth2;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aerogear_windows_oauth2_config
{
    public class GoogleConfig : Config
    {
        public async static Task<GoogleConfig> Create(string clientId, List<string> scopes, string accountId)
        {
            var protocol = await ManifestInfo.GetProtocol();
            return new GoogleConfig()
            {
                baseURL = "https://accounts.google.com/",
                authzEndpoint = "o/oauth2/auth",
                redirectURL = protocol + ":/oauth2Callback",
                accessTokenEndpoint = "o/oauth2/token",
                refreshTokenEndpoint = "o/oauth2/token",
                revokeTokenEndpoint = "rest/revoke",
                clientId = clientId,
                scopes = scopes,
                accountId = accountId
            };
        }
    }
}
