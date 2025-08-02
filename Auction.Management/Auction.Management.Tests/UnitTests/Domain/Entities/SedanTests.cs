using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Exceptions.Vehicle;
using Auction.Management.Domain.Enums;

namespace Auction.Management.Tests.UnitTests.Domain.Entities
{
    public class SedanTests
    {
        [Fact]
        public void Constructor_ShouldCreateSedan_WhenParametersAreValid()
        {
            var id = "1HGCM82633A004352";
            var manufacturer = "Honda";
            var model = "Accord";
            var year = 2021;
            var startingBid = 15000m;
            var numberOfDoors = 4;

            var sedan = new Sedan(id, manufacturer, model, year, startingBid, numberOfDoors);

            Assert.Equal(id, sedan.Id);
            Assert.Equal(manufacturer, sedan.Manufacturer);
            Assert.Equal(model, sedan.Model);
            Assert.Equal(year, sedan.Year);
            Assert.Equal(startingBid, sedan.StartingBid);
            Assert.Equal(numberOfDoors, sedan.NumberOfDoors);
            Assert.Equal(VehicleType.Sedan, sedan.GetVehicleType());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Constructor_ShouldThrowInvalidNumberOfDoorsException_WhenNumberOfDoorsIsLessThanTwo(int invalidNumberOfDoors)
        {
            var id = "1HGCM82633A004352";
            var manufacturer = "Honda";
            var model = "Accord";
            var year = 2021;
            var startingBid = 15000m;

            Assert.Throws<InvalidNumberOfDoorsException>(() =>
                new Sedan(id, manufacturer, model, year, startingBid, invalidNumberOfDoors));
        }
    }
}
