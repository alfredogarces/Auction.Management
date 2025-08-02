using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Exceptions.Vehicle;
using Auction.Management.Domain.Enums;

namespace Auction.Management.Tests.UnitTests.Domain.Entities
{
    public class SUVTests
    {
        [Fact]
        public void Constructor_ShouldCreateSUV_WhenParametersAreValid()
        {
            var id = "1HGCM82633A004352";
            var manufacturer = "Toyota";
            var model = "RAV4";
            var year = 2023;
            var startingBid = 30000m;
            var numberOfSeats = 5;

            var suv = new SUV(id, manufacturer, model, year, startingBid, numberOfSeats);

            Assert.Equal(id, suv.Id);
            Assert.Equal(manufacturer, suv.Manufacturer);
            Assert.Equal(model, suv.Model);
            Assert.Equal(year, suv.Year);
            Assert.Equal(startingBid, suv.StartingBid);
            Assert.Equal(numberOfSeats, suv.NumberOfSeats);
            Assert.Equal(VehicleType.SUV, suv.GetVehicleType());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Constructor_ShouldThrowInvalidNumberOfSeatsException_WhenNumberOfSeatsIsLessThanTwo(int invalidNumberOfSeats)
        {
            var id = "1HGCM82633A004352";
            var manufacturer = "Toyota";
            var model = "RAV4";
            var year = 2023;
            var startingBid = 30000m;

            Assert.Throws<InvalidNumberOfSeatsException>(() =>
                new SUV(id, manufacturer, model, year, startingBid, invalidNumberOfSeats));
        }
    }
}
