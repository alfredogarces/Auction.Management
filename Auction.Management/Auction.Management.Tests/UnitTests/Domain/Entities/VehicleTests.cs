using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Exceptions.Vehicle;
using Auction.Management.Domain.Enums;

namespace Auction.Management.Tests.UnitTests.Domain.Entities
{
    public class TestVehicle : Vehicle
    {
        public TestVehicle(string id, string manufacturer, string model, int year, decimal startingBid)
            : base(id, manufacturer, model, year, startingBid)
        {
        }

        public override Vehicle Clone()
        {
            return new TestVehicle(
                Id,
                Manufacturer,
                Model,
                Year,
                StartingBid
            );
        }

        public override VehicleType GetVehicleType()
        {
            return VehicleType.Sedan;
        }
    }

    public class VehicleTests
    {
        [Fact]
        public void Constructor_ShouldCreateVehicle_WhenParametersAreValid()
        {
            var id = "1HGCM82633A004352";
            var manufacturer = "Honda";
            var model = "Civic";
            var year = 2020;
            var startingBid = 15000m;

            var vehicle = new TestVehicle(id, manufacturer, model, year, startingBid);

            Assert.Equal(id, vehicle.Id);
            Assert.Equal(manufacturer, vehicle.Manufacturer);
            Assert.Equal(model, vehicle.Model);
            Assert.Equal(year, vehicle.Year);
            Assert.Equal(startingBid, vehicle.StartingBid);
        }

        [Fact]
        public void Constructor_ShouldThrowInvalidVehicleIdException_WhenIdIsInvalid()
        {
            var invalidId = "INVALIDVIN1234567";
            Assert.Throws<InvalidVehicleIdException>(() =>
                new TestVehicle(invalidId, "Honda", "Civic", 2020, 15000m));
        }

        [Fact]
        public void Constructor_ShouldThrowInvalidManufacturerException_WhenManufacturerIsNullOrWhitespace()
        {
            Assert.Throws<InvalidManufacturerException>(() =>
                new TestVehicle("1HGCM82633A004352", "", "Civic", 2020, 15000m));
        }

        [Fact]
        public void Constructor_ShouldThrowInvalidModelException_WhenModelIsNullOrWhitespace()
        {
            Assert.Throws<InvalidModelException>(() =>
                new TestVehicle("1HGCM82633A004352", "Honda", " ", 2020, 15000m));
        }

        [Theory]
        [InlineData(1800)]
        [InlineData(3000)] // ano futuro além do permitido
        public void Constructor_ShouldThrowInvalidYearException_WhenYearIsOutOfRange(int invalidYear)
        {
            Assert.Throws<InvalidYearException>(() =>
                new TestVehicle("1HGCM82633A004352", "Honda", "Civic", invalidYear, 15000m));
        }

        [Fact]
        public void Constructor_ShouldThrowInvalidStartingBidException_WhenStartingBidIsNegative()
        {
            Assert.Throws<InvalidStartingBidException>(() =>
                new TestVehicle("1HGCM82633A004352", "Honda", "Civic", 2020, -1m));
        }
    }
}
