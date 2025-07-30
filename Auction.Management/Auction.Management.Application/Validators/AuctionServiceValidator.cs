using Auction.Management.Domain.Exceptions.Auction;

namespace Auction.Management.Application.Validators
{
    public static class AuctionServiceValidator
    {
        public static void ValidateStartAuction(Domain.Entities.Auction? existingAuction)
        {
            if (existingAuction != null && existingAuction.IsActive)
                throw new AuctionAlreadyStartedException();
        }

        public static void ValidatePlaceBid(Domain.Entities.Auction auction, decimal bidAmount)
        {
            if (!auction.IsActive)
                throw new AuctionNotActiveException();

            decimal minimumBid = auction.HighestBid?.Amount ?? auction.Vehicle.StartingBid;
            if (bidAmount <= minimumBid)
                throw new BidTooLowException(bidAmount, minimumBid);
        }
    }

}
