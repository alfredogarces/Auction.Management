using Auction.Management.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Auction.Management.Domain.Validators
{
    public static class BidderValidator
    {
        public static void Validate(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                throw new InvalidEmailException(email);
        }
    }
}
