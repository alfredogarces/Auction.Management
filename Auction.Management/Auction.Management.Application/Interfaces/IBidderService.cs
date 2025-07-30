using Auction.Management.Application.Common;
using Auction.Management.Application.Dto;

namespace Auction.Management.Application.Interfaces
{
    public interface IBidderService
    {
        Task<Result<BidderDto>> GetByEmailAsync(string email);
        Task<Result<BidderDto>> AddBidderAsync(BidderDto bidderDto);
    }
}
