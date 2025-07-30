using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Exceptions.Auction;

namespace Auction.Management.Domain.Validators
{
    public static class AuctionValidator
    {
        public static void Validate(Vehicle vehicle)
        {
            if (vehicle is null)
                throw new InvalidAuctionVehicleException();
        }
    }
}
