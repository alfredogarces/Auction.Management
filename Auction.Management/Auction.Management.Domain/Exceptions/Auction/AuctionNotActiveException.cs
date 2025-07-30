namespace Auction.Management.Domain.Exceptions.Auction
{
    public class AuctionNotActiveException : InvalidOperationException
    {
        public AuctionNotActiveException()
            : base("Auction is not active.") { }
    }
}
