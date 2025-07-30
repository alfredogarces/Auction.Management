namespace Auction.Management.Domain.Exceptions.Vehicle
{
    public class InvalidModelException : ArgumentException
    {
        public InvalidModelException()
            : base("Model cannot be null or empty.")
        {
        }
    }
}
