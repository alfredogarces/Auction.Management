namespace Auction.Management.Domain.Exceptions
{
    public class InvalidBidderException : ArgumentException
    {
        public InvalidBidderException()
            : base("Bidder cannot be null.")
        {
        }
    }

}
