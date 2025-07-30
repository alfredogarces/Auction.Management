using Auction.Management.Domain.Interfaces;

namespace Auction.Management.Infrastructure.InMemoryRepositories
{
    public class InMemoryAuctionRepository : IAuctionRepository
    {
        private readonly List<Domain.Entities.Auction> _auctions = new();

        public Task Add(Domain.Entities.Auction auction)
        {
            _auctions.Add(auction);
            return Task.CompletedTask;
        }

        public Task<Domain.Entities.Auction?> GetByVehicleId(string vehicleId)
        {
            var auction = _auctions.FirstOrDefault(a => a.Vehicle.Id == vehicleId);
            return Task.FromResult(auction);
        }

        public Task<IEnumerable<Domain.Entities.Auction>> GetAll()
        {
            return Task.FromResult<IEnumerable<Domain.Entities.Auction>>(_auctions.ToList());
        }

        public async Task Update(Domain.Entities.Auction auction)
        {
            var existing = await GetByVehicleId(auction.Vehicle.Id);
            if (existing != null)
            {
                _auctions.Remove(existing);
                _auctions.Add(auction);
            }
        }
    }
}
