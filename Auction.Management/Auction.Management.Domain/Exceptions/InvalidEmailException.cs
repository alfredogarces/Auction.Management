namespace Auction.Management.Domain.Exceptions
{
    /// <summary>
    /// Thrown when an invalid email address is provided.
    /// </summary>
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string email)
            : base($"Invalid email address: '{email}'.")
        {
        }
    }
}
