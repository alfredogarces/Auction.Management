using Auction.Management.Domain.Entities;

namespace Auction.Management.Domain.Interfaces
{
    public interface IBidderRepository
    {
        Task<bool> AddAsync(Bidder bidder);
        Task<Bidder?> GetByEmailAsync(string email);
        Task<IEnumerable<Bidder>> GetAllAsync();
    }
}
