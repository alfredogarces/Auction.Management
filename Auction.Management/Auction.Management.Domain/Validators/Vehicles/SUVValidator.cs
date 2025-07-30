using Auction.Management.Domain.Exceptions.Vehicle;

namespace Auction.Management.Domain.Validators.Vehicles
{
    public static class SUVValidator
    {
        public static void Validate(int numberOfSeats)
        {
            if (numberOfSeats < 2)
                throw new InvalidNumberOfSeatsException(numberOfSeats);
        }
    }
}
