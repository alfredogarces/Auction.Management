using Auction.Management.Domain.Enums;
using Auction.Management.Domain.Validators.Vehicles;

namespace Auction.Management.Domain.Entities.Vehicles
{
    public class Sedan : Vehicle
    {
        public int NumberOfDoors { get; private set; }

        public Sedan(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            SedanValidator.Validate(numberOfDoors);
            NumberOfDoors = numberOfDoors;
        }

        public override VehicleType GetVehicleType()
        {
            return VehicleType.Sedan;
        }
    }
}
