using Dynamics365WebAPI.Options;
using Dynamics365WebAPI.Services;
using Dynamics365WebAPI.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0#disable-problemdetails-response
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressMapClientErrors = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Generic Dynamics 365 Web API",
        Description = "An ASP.NET Core Web API for middleware between D365 and another software.",
    });

});

builder.Services.AddScoped<IDynamics365Repository, Dynamics365Repository>();
builder.Services.AddScoped<IDynamics365Service, Dynamics365Service>();
//builder.Services.AddScoped<IWebApiQueryBuilderHelper, WebApiQueryBuilderHelper>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddMemoryCache();

//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0
//this uses the appsettings.json information
builder.Services.Configure<Dynamics365WebApiOptions>(
    builder.Configuration.GetSection(Dynamics365WebApiOptions.Dynamics365WebApi));
builder.Services.Configure<ConfigurationOptions>(
    builder.Configuration.GetSection(ConfigurationOptions.Configuration));

var app = builder.Build();

//https://www.talkingdotnet.com/add-swagger-to-asp-net-core-6-app/
//https://stackoverflow.com/questions/57674083/swagger-ui-not-generating-in-azure-net-core-but-it-is-working-in-local
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

