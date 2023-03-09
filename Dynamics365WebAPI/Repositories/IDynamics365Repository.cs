namespace Dynamics365WebAPI.Repositories
{
    /// <summary>
    /// CRUD operations using the Dynamics 365 Web Api.
    /// </summary>
    public interface IDynamics365Repository
    {
        /// <summary>
        /// Gets records from Dynamics 365 Web Api. It throws an exception if it receives a non-200 Http Status code.
        /// </summary>
        /// <param name="apiQuery">This is the parameter query string that is after the https://dynamicsorg.api.crm.dynamics.com/api/data/v9.2/</param>
        /// <returns></returns>
        Task<HttpResponseMessage> Get(string apiQuery);

        /// <summary>
        /// Updates record(s) with Dynamics 365 Web API. It throws an exception if it receives a non-200 Http Status code.
        /// </summary>
        /// <param name="apiQuery">This is the parameter query string that is after the https://dynamicsorg.api.crm.dynamics.com/api/data/v9.2/</param>
        /// <returns></returns>
        Task<HttpResponseMessage> Update(string entityPluralName, HttpContent httpContentPayload);

        /// <summary>
        /// Creates record(s) from Dynamics 365 Web API. It throws an exception if it receives a non-200 Http Status code.
        /// </summary>
        /// <param name="apiQuery">This is the parameter query string that is after the https://dynamicsorg.api.crm.dynamics.com/api/data/v9.2/</param>
        /// <returns></returns>
        Task<HttpResponseMessage> Create(string entityPluralName, HttpContent httpContentPayload);
      
    }
}
