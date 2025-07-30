using Auction.Management.Domain.Enums;
using Auction.Management.Domain.Validators.Vehicles;

namespace Auction.Management.Domain.Entities.Vehicles
{
    public class Truck : Vehicle
    {
        public double LoadCapacity { get; private set; }

        public Truck(string id, string manufacturer, string model, int year, decimal startingBid, double loadCapacity)
            : base(id, manufacturer, model, year, startingBid)
        {
            TruckValidator.Validate(loadCapacity);
            LoadCapacity = loadCapacity;
        }

        public override VehicleType GetVehicleType()
        {
            return VehicleType.Truck;
        }
    }

}
