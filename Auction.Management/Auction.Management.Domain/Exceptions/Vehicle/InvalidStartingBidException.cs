namespace Auction.Management.Domain.Exceptions.Vehicle
{
    public class InvalidStartingBidException : ArgumentException
    {
        public InvalidStartingBidException(decimal startingBid)
            : base($"Starting bid '{startingBid}' is invalid. It must be zero or greater.")
        {
        }
    }
}
