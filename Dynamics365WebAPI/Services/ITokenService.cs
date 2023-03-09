using Microsoft.Identity.Client;

namespace Dynamics365WebAPI.Services
{
    /// <summary>
    /// Service to get the OAuth 2.0 access token from Dynamics 365.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Gets the authentication access token using an App Registration and MSAL.NET.
        /// </summary>
        Task<AuthenticationResult> FetchAccessToken();

    }
}
