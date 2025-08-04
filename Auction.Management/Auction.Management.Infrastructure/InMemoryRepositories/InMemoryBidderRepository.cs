using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Interfaces;

namespace Auction.Management.Infrastructure.InMemoryRepositories
{
    public class InMemoryBidderRepository : IBidderRepository
    {
        private readonly List<Bidder> _bidders = new();

        public Task AddAsync(Bidder bidder)
        {
            _bidders.Add(bidder.Clone());
            return Task.CompletedTask;
        }

        public Task<Bidder?> GetByEmailAsync(string email)
        {
            var bidder = _bidders.FirstOrDefault(b => b.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(bidder?.Clone());
        }

        public Task<IEnumerable<Bidder>> GetAllAsync()
        {
            var clones = _bidders.Select(b => b.Clone()).ToList();
            return Task.FromResult<IEnumerable<Bidder>>(clones);
        }
    }
}
