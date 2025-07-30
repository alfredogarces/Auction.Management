namespace Auction.Management.Domain.Exceptions.Auction
{
    public class BidTooLowException : ArgumentException
    {
        public BidTooLowException(decimal amount, decimal minimum)
            : base($"Bid amount '{amount}' is too low. Minimum required is '{minimum}'.") { }
    }
}
