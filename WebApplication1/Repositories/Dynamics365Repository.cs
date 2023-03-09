using Dynamics365WebAPI.Options;
using Dynamics365WebAPI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Dynamics365WebAPI.Repositories
{
    /// <inheritdoc />
    public class Dynamics365Repository : IDynamics365Repository
    {
        private ITokenService _tokenService { get; set; }
        private Dynamics365WebApiOptions _Dynamics365WebApiOptions { get; set; }

        public Dynamics365Repository(ITokenService tokenService, IOptions<Dynamics365WebApiOptions> Dynamics365WebApiOptions)
        {
            _tokenService = tokenService;
            _Dynamics365WebApiOptions = Dynamics365WebApiOptions.Value;
        }
        
        /// <summary>
        /// Creates a default HttpClient for standard GET Web API calls with the access token.
        /// </summary>
        /// <param name="token">Access token used with the Bearer header.</param>
        /// <returns></returns>
        private HttpClient CreateGetHttpClient(AuthenticationResult token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            client.DefaultRequestHeaders.Add("OData-Version", "4.0");
            client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=*");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return client;
        }

        /// <summary>
        /// Creates a default HttpClient for standard Web CREATE/PATCH API calls with the access token.
        /// </summary>
        /// <param name="token">Access token used with the Bearer header.</param>
        /// <returns></returns>
        private HttpClient CreatePostPatchHttpClient(AuthenticationResult token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            client.DefaultRequestHeaders.Add("OData-Version", "4.0");
            client.DefaultRequestHeaders.Add("Prefer", "return=representation");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return client;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> Get(string apiQuery)
        {
            AuthenticationResult token = await _tokenService.FetchAccessToken();
            var client = CreateGetHttpClient(token);
            var jsonResult = await client.GetAsync(_Dynamics365WebApiOptions.ServiceRoot + apiQuery);
            client.Dispose();
            if (jsonResult.IsSuccessStatusCode)
            {
                return jsonResult;
            }
            else
            {
                //we use InternalServerError becuase the Dynamics 365 API will tell us it's a 400 error and that makes no sense to an external caller like AEG the vendor
                throw new BadHttpRequestException("Error Code: 00005 - GET Request Failed: " + jsonResult.Content.ReadAsStringAsync().Result, 500);
            }
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> Create(string entityPluralName, HttpContent httpContentPayload)
        {
            AuthenticationResult token = await _tokenService.FetchAccessToken();
            var client = CreatePostPatchHttpClient(token);
            var jsonResult = await client.PostAsync(_Dynamics365WebApiOptions.ServiceRoot + entityPluralName, httpContentPayload);
            client.Dispose();
            if (jsonResult.IsSuccessStatusCode)
            {
                return jsonResult;
            }
            else
            {
                var test = jsonResult.Content.ReadAsStringAsync().Result;
                //we use InternalServerError becuase the Dynamics 365 API will tell us it's a 400 error and that makes no sense to an external caller like AEG the vendor
                throw new BadHttpRequestException("Error Code: 00006 - POST Request Failed: " + jsonResult.Content.ReadAsStringAsync().Result, 500);
            }
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> Update(string entityPluralName, HttpContent httpContentPayload)
        {
            AuthenticationResult token = await _tokenService.FetchAccessToken();
            var client = CreatePostPatchHttpClient(token);
            var jsonResult = await client.PatchAsync(_Dynamics365WebApiOptions.ServiceRoot + entityPluralName, httpContentPayload);
            client.Dispose();
            if (jsonResult.IsSuccessStatusCode)
            {
                return jsonResult;
            }
            else
            {
                //we use InternalServerError becuase the Dynamics 365 API will tell us it's a 400 error and that makes no sense to an external caller like AEG the vendor
                throw new BadHttpRequestException("Error Code: 00007 - PATCH Request Failed: " + jsonResult.Content.ReadAsStringAsync().Result, 500);
            }
        }


    }
}
