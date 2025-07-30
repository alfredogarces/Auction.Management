using Auction.Management.Domain.Exceptions.Vehicle;

namespace Auction.Management.Domain.Validators.Vehicles
{
    public static class SedanValidator
    {
        public static void Validate(int numberOfDoors)
        {
            if (numberOfDoors < 2)
                throw new InvalidNumberOfDoorsException(numberOfDoors);
        }
    }

}
