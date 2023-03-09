namespace Dynamics365WebAPI.Options
{
    /// <summary>
    /// Uses the .NET Core 6.0 Options pattern for accessing appsettings.json configuration.
    /// </summary>
    public class Dynamics365WebApiOptions
    {
        public const string Dynamics365WebApi = "Dynamics365WebApi";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string ResourceUri { get; set; }
        public string ServiceRoot { get; set; }
        public string AuthorityUri { get; set; }
    }
}
