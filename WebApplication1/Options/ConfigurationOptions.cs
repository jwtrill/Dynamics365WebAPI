namespace Dynamics365WebAPI.Options
{
    /// <summary>
    /// Uses the .NET Core 6.0 Options pattern for accessing appsettings.json configuration.
    /// </summary>
    public class ConfigurationOptions
    {
        public const string Configuration = "Configuration";
        public string ApiKey { get; set; }
    }
}
