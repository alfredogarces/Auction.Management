using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Exceptions;

namespace Auction.Management.Domain.Validators
{
    public static class BidValidator
    {
        public static void Validate(Bidder bidder, decimal amount)
        {
            if (bidder is null)
                throw new InvalidBidderException();

            if (amount <= 0)
                throw new InvalidBidAmountException(amount);
        }
    }
}
