using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Dynamic;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;
using Microsoft.Extensions.Options;
using Dynamics365WebAPI.Services;
using Dynamics365WebAPI.Options;

namespace Dynamics365WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/generic")]
    public class GenericController : ControllerBase
    {

        private readonly ILogger<GenericController> _logger;
        private IDynamics365Service _dynamics365Service;
        private ConfigurationOptions _configurationOptions;

        public GenericController(IDynamics365Service dynamics365Service, ILogger<GenericController> logger, IOptions<ConfigurationOptions> configurationOptions)
        {
            _dynamics365Service = dynamics365Service;
            //https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line
            //this logger isn't fully implemented into anything by default from what I see...
            _logger = logger;
            _configurationOptions = configurationOptions.Value;
        }

        //https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio
        /// <summary>
        /// This is a generic GET request as an example template.
        /// </summary>
        /// <param name="key">Key required for GET request.</param>
        /// <returns>Some records you make it return.</returns>
        /// <response code="200">GET request is successful.</response>
        /// <response code="401">Unauthorized.</response>
        [HttpGet("test")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStuff(string key) //adds a key string parameter to the URL
        {
            if (key != _configurationOptions.ApiKey)
            {
                return StatusCode(401, "Unauthorized!");
            }
            var response = _dynamics365Service.SomeMethod();
            return StatusCode(200, response);
        }

    }
}