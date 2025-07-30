using Auction.Management.Application.Interfaces;
using Auction.Management.Application.Services;
using Auction.Management.Domain.Interfaces;
using Auction.Management.Infrastructure.InMemoryRepositories;

namespace Auction.Management.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependecyInjection(this IServiceCollection services)
        {
            return services
                .AddScoped<IVehicleService, VehicleService>()
                .AddScoped<IBidderService, BidderService>()
                .AddScoped<IAuctionService, AuctionService>()
                .AddSingleton<IAuctionRepository, InMemoryAuctionRepository>()
                .AddSingleton<IBidderRepository, InMemoryBidderRepository>()
                .AddSingleton<IVehicleRepository, InMemoryVehicleRepository>();
        }
    }
}

