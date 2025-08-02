using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Exceptions;

namespace Auction.Management.Tests.UnitTests.Domain.Entities
{
    public class BidTests
    {
        private Bidder CreateValidBidder() =>
            new Bidder("valid@email.com");

        [Fact]
        public void Constructor_ShouldCreateBid_WhenBidderAndAmountAreValid()
        {
            var bidder = CreateValidBidder();
            var amount = 10000;

            var bid = new Bid(bidder, amount);

            Assert.Equal(bidder, bid.Bidder);
            Assert.Equal(amount, bid.Amount);
        }

        [Fact]
        public void Constructor_ShouldThrowInvalidBidderException_WhenBidderIsNull()
        {
            Assert.Throws<InvalidBidderException>(() => new Bid(null!, 10000));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Constructor_ShouldThrowInvalidBidAmountException_WhenAmountIsZeroOrNegative(decimal invalidAmount)
        {
            var bidder = CreateValidBidder();

            Assert.Throws<InvalidBidAmountException>(() => new Bid(bidder, invalidAmount));
        }
    }
}
