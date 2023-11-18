using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.ConfigureLogging(log => log.AddConsole());

            // add ocelot to DI container
            builder.Services.AddOcelot();

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            // add ocelot middlaware
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}