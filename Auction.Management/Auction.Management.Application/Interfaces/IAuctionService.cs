using Auction.Management.Application.Common;
using Auction.Management.Application.Dto;

namespace Auction.Management.Application.Interfaces
{
    public interface IAuctionService
    {
        Task<Result<AuctionDto>> StartAuctionAsync(string vehicleId);
        Task<Result<AuctionDto>> CloseAuctionAsync(string vehicleId);
        Task<Result<AuctionDto>> PlaceBidAsync(string vehicleId, BidDto bidDto);
    }
}
