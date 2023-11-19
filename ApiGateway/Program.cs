using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // var env = builder.Environment.EnvironmentName;
            // var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            builder.Configuration.AddJsonFile($"ocelot.json", optional: false, reloadOnChange: true);

            builder.Host.ConfigureLogging(log => log.AddConsole());

            // configure authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => 
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:PrivateKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // add ocelot to DI container
            builder.Services.AddOcelot(builder.Configuration);

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.UseAuthentication();
            // app.UseAuthorization();

            // add ocelot middlaware
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}