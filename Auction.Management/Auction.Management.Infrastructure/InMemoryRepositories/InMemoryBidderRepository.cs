using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Interfaces;
using System.Collections.Concurrent;

namespace Auction.Management.Infrastructure.InMemoryRepositories
{
    public class InMemoryBidderRepository : IBidderRepository
    {
        private readonly ConcurrentDictionary<string, Bidder> _bidders = new();

        public Task<bool> AddAsync(Bidder bidder)
        {
            bool added = _bidders.TryAdd(bidder.Email.ToLowerInvariant(), bidder.Clone());
            return Task<bool>.FromResult(added);
        }

        public Task<Bidder?> GetByEmailAsync(string email)
        {
            _bidders.TryGetValue(email.ToLowerInvariant(), out var bidder);
            return Task.FromResult(bidder?.Clone());
        }

        public Task<IEnumerable<Bidder>> GetAllAsync()
        {
            var clones = _bidders.Values.Select(b => b.Clone()).ToList();
            return Task.FromResult<IEnumerable<Bidder>>(clones);
        }
    }
}
