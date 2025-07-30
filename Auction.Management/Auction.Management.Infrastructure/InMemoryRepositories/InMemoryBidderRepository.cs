using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Interfaces;

namespace Auction.Management.Infrastructure.InMemoryRepositories
{
    public class InMemoryBidderRepository : IBidderRepository
    {
        private readonly List<Bidder> _bidders = new();

        public Task AddAsync(Bidder bidder)
        {
            _bidders.Add(bidder);
            return Task.CompletedTask;
        }

        public Task<Bidder?> GetByEmailAsync(string email)
        {
            var bidder = _bidders.FirstOrDefault(b => b.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(bidder);
        }

        public Task<IEnumerable<Bidder>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Bidder>>(_bidders.ToList());
        }
    }
}
