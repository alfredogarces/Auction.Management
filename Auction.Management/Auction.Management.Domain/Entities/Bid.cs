
using Auction.Management.Domain.Validators;

namespace Auction.Management.Domain.Entities
{
    public class Bid
    {
        public Bidder Bidder { get; set; }
        public decimal Amount { get; set; }

        public Bid(Bidder bidder, decimal amount)
        {
            BidValidator.Validate(bidder, amount);
            Bidder = bidder;
            Amount = amount;
        }
    }
}