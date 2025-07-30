namespace Auction.Management.Domain.Exceptions.Vehicle
{
    public class InvalidManufacturerException : ArgumentException
    {
        public InvalidManufacturerException()
            : base("Manufacturer cannot be null or empty.")
        {
        }
    }
}
