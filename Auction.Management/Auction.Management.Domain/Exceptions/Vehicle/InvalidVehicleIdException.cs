namespace Auction.Management.Domain.Exceptions.Vehicle
{
    public class InvalidVehicleIdException : ArgumentException
    {
        public InvalidVehicleIdException()
            : base("Vehicle ID cannot be null or empty.")
        {
        }
    }
}
