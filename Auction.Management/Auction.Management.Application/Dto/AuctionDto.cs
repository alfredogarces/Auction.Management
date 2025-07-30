namespace Auction.Management.Application.Dto
{
    public record AuctionDto(string VehicleId, bool IsActive, BidDto? HighestBid);
}
