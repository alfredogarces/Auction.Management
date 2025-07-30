using Auction.Management.Domain.Enums;
using Auction.Management.Domain.Validators.Vehicles;

namespace Auction.Management.Domain.Entities.Vehicles
{
    public abstract class Vehicle
    {
        public string Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal StartingBid { get; set; }

        protected Vehicle(string id, string manufacturarer, string model, int year, decimal startingBid)
        {
            VehicleValidator.Validate(id, manufacturarer, model, year, startingBid);
            Id = id;
            Manufacturer = manufacturarer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
        }

        public abstract VehicleType GetVehicleType();
    }

}
