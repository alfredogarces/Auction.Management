using Auction.Management.Domain.Enums;
using Auction.Management.Domain.Validators.Vehicles;

namespace Auction.Management.Domain.Entities.Vehicles
{
    public class Hatchback : Vehicle
    {
        public int NumberOfDoors { get; private set; }

        public Hatchback(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            HatchbackValidator.Validate(numberOfDoors);
            NumberOfDoors = numberOfDoors;
        }

        public override VehicleType GetVehicleType()
        {
            return VehicleType.Hatchback;
        }

        public override Vehicle Clone()
        {
            return new Hatchback(
                Id,
                Manufacturer,
                Model,
                Year,
                StartingBid,
                NumberOfDoors
            );
        }

    }

}
