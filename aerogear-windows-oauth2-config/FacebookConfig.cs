using AeroGear.OAuth2;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace aerogear_windows_oauth2_config
{
    public class FacebookConfig : Config
    {
        public static FacebookConfig Create(string clientId, string clientSecret, List<string> scopes, string accountId)
        {
            return new FacebookConfig()
            {
                baseURL = "",
                authzEndpoint = "https://www.facebook.com/dialog/oauth",
                redirectURL = "fb" + clientId + "://authorize/",
                accessTokenEndpoint = "https://graph.facebook.com/oauth/access_token",
                clientId = clientId,
                refreshTokenEndpoint = "https://graph.facebook.com/oauth/access_token",
                clientSecret = clientSecret,
                revokeTokenEndpoint = "https://www.facebook.com/me/permissions",
                scopes = scopes,
                accountId = accountId
            };
        }
    }

    public class FacebookOAuth2Module : OAuth2Module
    {
        public async static new Task<OAuth2Module> Create(Config config)
        {
            FacebookOAuth2Module module = new FacebookOAuth2Module();
            await module.init(config);
            AccountManager.RegisterModule(config, module);
            return module;
        }

        protected override async Task<Session> ParseResponse(Stream respondeStream)
        {
            StreamReader reader = new StreamReader(respondeStream);
            string queryString = await reader.ReadToEndAsync();
            IDictionary<string, string> data = ParseQueryString(queryString);

            Session session = new Session()
            {
                accessToken = data["access_token"],
                accessTokenExpiration = int.Parse(data["expires"])
            };
            return session;
        }
    }

}
