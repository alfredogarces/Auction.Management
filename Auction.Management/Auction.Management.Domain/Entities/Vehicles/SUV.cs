using Auction.Management.Domain.Enums;
using Auction.Management.Domain.Validators.Vehicles;

namespace Auction.Management.Domain.Entities.Vehicles
{
    public class SUV : Vehicle
    {
        public int NumberOfSeats { get; private set; }

        public SUV(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfSeats)
            : base(id, manufacturer, model, year, startingBid)
        {
            SUVValidator.Validate(numberOfSeats);
            NumberOfSeats = numberOfSeats;
        }

        public override VehicleType GetVehicleType()
        {
            return VehicleType.SUV;
        }

        public override Vehicle Clone()
        {
            return new SUV(
                Id,
                Manufacturer,
                Model,
                Year,
                StartingBid,
                NumberOfSeats
            );
        }

    }

}