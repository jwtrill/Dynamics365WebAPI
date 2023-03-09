using Dynamics365WebAPI.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace Dynamics365WebAPI.Services
{
    /// <inheritdoc />
    public class TokenService : ITokenService
    {
        //ripped Single-tenant-S2S and QuickStart-MSAL from https://github.com/microsoft/PowerApps-Samples for token request
        private Dynamics365WebApiOptions _dynamics365WebApiOptions;

        public TokenService(IOptions<Dynamics365WebApiOptions> dynamics365WebApi)
        {
            _dynamics365WebApiOptions = dynamics365WebApi.Value;
        }

        /// <inheritdoc />
        public async Task<AuthenticationResult> FetchAccessToken()
        {
            try
            {
                //https://www.1clickfactory.com/blog/how-to-authenticate-through-azure-active-directory-to-use-business-central-api/ got the structure from here
                //web api is a confidentialclientapp https://learn.microsoft.com/en-us/azure/active-directory/develop/msal-client-applications
                var confidentialClient = ConfidentialClientApplicationBuilder
                  .Create(_dynamics365WebApiOptions.ClientId)
                  .WithClientSecret(_dynamics365WebApiOptions.ClientSecret)
                  .WithAuthority(new Uri(_dynamics365WebApiOptions.AuthorityUri + _dynamics365WebApiOptions.TenantId))
                  .Build();

                //https://medium.com/capgemini-microsoft-team/access-tokens-for-dynamics-365-using-microsoft-authentication-library-2b16c9f794b
                var scopes = new string[] { _dynamics365WebApiOptions.ResourceUri + "/.default" };
                //this seems to reuse and get new tokens automatically
                //https://learn.microsoft.com/en-us/azure/active-directory/develop/msal-net-acquire-token-silently
                var token = await confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync();

                return token;
            }
            catch
            {
                throw new BadHttpRequestException("Error Code: 00009 - Authorization has failed.", 401);
            }
        }

    }
}
