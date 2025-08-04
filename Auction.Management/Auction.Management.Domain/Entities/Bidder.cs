using Auction.Management.Domain.Validators;
using Auction.Management.Domain.Exceptions;

namespace Auction.Management.Domain.Entities
{
    public class Bidder
    {
        public string Email { get; private set; }

        public Bidder(string email)
        {
            BidderValidator.Validate(email);
            Email = email;
        }

        public Bidder Clone()
        {
            return new Bidder(Email);
        }
    }
}
