namespace Auction.Management.Domain.Exceptions.Auction
{
    public class AuctionAlreadyStartedException : InvalidOperationException
    {
        public AuctionAlreadyStartedException()
            : base("Auction has already started.") { }
    }
}
