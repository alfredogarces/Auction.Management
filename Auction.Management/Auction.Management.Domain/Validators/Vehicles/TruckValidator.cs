using Auction.Management.Domain.Exceptions.Vehicle;


namespace Auction.Management.Domain.Validators.Vehicles
{
    internal class TruckValidator
    {
        public static void Validate(double loadCapacity)
        {
            if (loadCapacity <= 0)
                throw new InvalidLoadCapacityException(loadCapacity);
        }
    }
}
