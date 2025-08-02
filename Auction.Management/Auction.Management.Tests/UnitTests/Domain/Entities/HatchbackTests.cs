using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Exceptions.Vehicle;
using Auction.Management.Domain.Enums;
using Xunit;

namespace Auction.Management.Tests.UnitTests.Domain.Entities
{
    public class HatchbackTests
    {
        [Fact]
        public void Constructor_ShouldCreateHatchback_WhenParametersAreValid()
        {
            // Arrange
            var id = "1HGCM82633A004352";
            var manufacturer = "Toyota";
            var model = "Corolla";
            var year = 2020;
            var startingBid = 10000m;
            var numberOfDoors = 4;

            // Act
            var hatchback = new Hatchback(id, manufacturer, model, year, startingBid, numberOfDoors);

            // Assert
            Assert.Equal(id, hatchback.Id);
            Assert.Equal(manufacturer, hatchback.Manufacturer);
            Assert.Equal(model, hatchback.Model);
            Assert.Equal(year, hatchback.Year);
            Assert.Equal(startingBid, hatchback.StartingBid);
            Assert.Equal(numberOfDoors, hatchback.NumberOfDoors);
            Assert.Equal(VehicleType.Hatchback, hatchback.GetVehicleType());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Constructor_ShouldThrowInvalidNumberOfDoorsException_WhenNumberOfDoorsIsLessThanTwo(int invalidNumberOfDoors)
        {
            var id = "1HGCM82633A004352";
            var manufacturer = "Toyota";
            var model = "Corolla";
            var year = 2020;
            var startingBid = 10000m;

            Assert.Throws<InvalidNumberOfDoorsException>(() =>
                new Hatchback(id, manufacturer, model, year, startingBid, invalidNumberOfDoors));
        }
    }
}
