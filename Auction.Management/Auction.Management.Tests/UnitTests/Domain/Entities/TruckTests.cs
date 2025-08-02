using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Exceptions.Vehicle;
using Auction.Management.Domain.Enums;

namespace Auction.Management.Tests.UnitTests.Domain.Entities
{
    public class TruckTests
    {
        [Fact]
        public void Constructor_ShouldCreateTruck_WhenParametersAreValid()
        {
            var id = "1HGCM82633A004352";
            var manufacturer = "Volvo";
            var model = "FH16";
            var year = 2022;
            var startingBid = 50000m;
            var loadCapacity = 10.5;

            var truck = new Truck(id, manufacturer, model, year, startingBid, loadCapacity);

            Assert.Equal(id, truck.Id);
            Assert.Equal(manufacturer, truck.Manufacturer);
            Assert.Equal(model, truck.Model);
            Assert.Equal(year, truck.Year);
            Assert.Equal(startingBid, truck.StartingBid);
            Assert.Equal(loadCapacity, truck.LoadCapacity);
            Assert.Equal(VehicleType.Truck, truck.GetVehicleType());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Constructor_ShouldThrowInvalidLoadCapacityException_WhenLoadCapacityIsZeroOrNegative(double invalidLoadCapacity)
        {
            var id = "1HGCM82633A004352";
            var manufacturer = "Volvo";
            var model = "FH16";
            var year = 2022;
            var startingBid = 50000m;

            Assert.Throws<InvalidLoadCapacityException>(() =>
                new Truck(id, manufacturer, model, year, startingBid, invalidLoadCapacity));
        }
    }
}
