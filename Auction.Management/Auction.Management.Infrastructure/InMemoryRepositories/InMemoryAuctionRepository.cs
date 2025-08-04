using Auction.Management.Domain.Interfaces;
using System.Collections.Concurrent;

namespace Auction.Management.Infrastructure.InMemoryRepositories
{
    public class InMemoryAuctionRepository : IAuctionRepository
    {
        private readonly ConcurrentDictionary<string, Domain.Entities.Auction> _auctions = new();

        public Task Add(Domain.Entities.Auction auction)
        {
            _auctions.TryAdd(auction.Vehicle.Id, auction.Clone());
            return Task.CompletedTask;
        }

        public Task<Domain.Entities.Auction?> GetByVehicleId(string vehicleId)
        {
            _auctions.TryGetValue(vehicleId, out var auction);
            return Task.FromResult(auction?.Clone());
        }

        public Task<IEnumerable<Domain.Entities.Auction>> GetAll()
        {
            var clones = _auctions.Values.Select(a => a.Clone()).ToList();
            return Task.FromResult<IEnumerable<Domain.Entities.Auction>>(clones);
        }

        public Task Update(Domain.Entities.Auction auction)
        {
            _auctions.AddOrUpdate(auction.Vehicle.Id, auction.Clone(), (key, old) => auction.Clone());
            return Task.CompletedTask;
        }
    }
}
