using Dynamics365WebAPI.Repositories;
using System.Net;
using System.Text.Json.Serialization;
using System.Reflection;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Text;
using System.Configuration;


namespace Dynamics365WebAPI.Services
{
    public class Dynamics365Service : IDynamics365Service
    {
        private ITokenService _tokenService;
        private IDynamics365Repository _dynamics365Repository;

        public Dynamics365Service(ITokenService tokenService, IDynamics365Repository dynamics365Repository)
        {
            _tokenService = tokenService;
            _dynamics365Repository = dynamics365Repository;
        }

        public string SomeMethod()
        {
            return "some method";
        }
    }

}
