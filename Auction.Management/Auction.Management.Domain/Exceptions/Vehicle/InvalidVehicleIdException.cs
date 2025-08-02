namespace Auction.Management.Domain.Exceptions.Vehicle
{
    public class InvalidVehicleIdException : ArgumentException
    {
        public InvalidVehicleIdException()
            : base("Invalid Vehicle ID")
        {
        }
    }
}
