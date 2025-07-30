
namespace Auction.Management.Domain.Exceptions
{
    public class InvalidBidAmountException : ArgumentException
    {
        public InvalidBidAmountException(decimal amount)
            : base($"Bid amount '{amount}' is invalid. It must be greater than zero.")
        {
        }
    }
}
