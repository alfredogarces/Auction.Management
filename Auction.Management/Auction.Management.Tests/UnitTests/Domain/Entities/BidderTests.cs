using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Exceptions;
using Xunit;

namespace Auction.Management.Tests.UnitTests.Domain.Entities
{
    public class BidderTests
    {
        [Fact]
        public void Constructor_ShouldCreateBidder_WhenEmailIsValid()
        {
            var email = "valid@email.com";
            var bidder = new Bidder(email);

            Assert.Equal(email, bidder.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("invalidemail")]
        [InlineData("invalid@")]
        [InlineData("@invalid.com")]
        [InlineData("test@invalid.")]
        public void Constructor_ShouldThrowInvalidEmailException_WhenEmailIsInvalid(string invalidEmail)
        {
            Assert.Throws<InvalidEmailException>(() => new Bidder(invalidEmail));
        }
    }
}
