using Auction.Management.Application.Interfaces;
using Auction.Management.Application.Services;
using Auction.Management.Domain.Interfaces;
using Auction.Management.Infrastructure.InMemoryRepositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.Management.Tests.IntegrationTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IVehicleRepository));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddScoped<IVehicleRepository, InMemoryVehicleRepository>();

                services.AddScoped<IVehicleService, VehicleService>();
            });
        }
    }
}