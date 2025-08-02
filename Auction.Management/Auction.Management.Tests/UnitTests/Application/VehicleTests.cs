using Auction.Management.Application.Dto;
using Auction.Management.Application.Services;
using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Enums;
using Auction.Management.Domain.Interfaces;
using AutoMapper;
using Moq;

namespace Auction.Management.Tests.UnitTests.Application
{


    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private VehicleService CreateService() => new(_vehicleRepositoryMock.Object, _mapperMock.Object);

        [Fact]
        public async Task AddVehicle_ShouldReturnFailure_WhenVehicleExists()
        {
            HatchbackDto vehicleDto = new HatchbackDto("AAAAAAAAAAAAAAAAA", "Ford", "Focus", 2020, 15000m, 4);
            Hatchback vehicle = new Hatchback("AAAAAAAAAAAAAAAAA", "Ford", "Focus", 2020, 15000m, 4);

            _mapperMock.Setup(m => m.Map<Vehicle>(vehicleDto)).Returns(vehicle);
            _vehicleRepositoryMock.Setup(r => r.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);

            var service = CreateService();

            var result = await service.AddVehicle(vehicleDto);

            Assert.True(result.IsFailure);
            Assert.Contains("already exists", result.Errors?.First().Description);
        }

        [Fact]
        public async Task AddVehicle_ShouldAddVehicle_WhenVehicleDoesNotExist()
        {
            var vehicleDto = new SedanDto("AAAAAAAAAAAAAAAAA", "Toyota", "Camry", 2021, 20000m, 4);
            var vehicle = new Sedan("AAAAAAAAAAAAAAAAA", "Toyota", "Camry", 2021, 20000m, 4);

            _mapperMock.Setup(m => m.Map<Vehicle>(vehicleDto)).Returns(vehicle);
            _vehicleRepositoryMock.Setup(r => r.GetByIdAsync(vehicle.Id)).ReturnsAsync((Vehicle?)null);
            _vehicleRepositoryMock.Setup(r => r.AddAsync(vehicle)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<VehicleDto>(vehicle)).Returns(vehicleDto);

            var service = CreateService();

            var result = await service.AddVehicle(vehicleDto);

            Assert.True(result.IsSuccess);
            Assert.Equal(vehicleDto.Id, result.Data?.Id);
            _vehicleRepositoryMock.Verify(r => r.AddAsync(vehicle), Times.Once);
        }

        [Fact]
        public async Task SearchAsync_ShouldFilterVehiclesByTypeManufacturerModelYear()
        {
            var vehicles = new List<Vehicle>
        {
            new Sedan("AAAAAAAAAAAAAAAAA", "Toyota", "Camry", 2021, 20000m, 4),
            new Hatchback("AAAAAAAAAAAAAAAA1", "Ford", "Focus", 2020, 15000m, 4),
            new Sedan("AAAAAAAAAAAAAAAA3", "Toyota", "Corolla", 2019, 18000m, 4),
        };

            _vehicleRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(vehicles);

            var sedanDto = new SedanDto("AAAAAAAAAAAAAAAAA", "Toyota", "Camry", 2021, 20000m, 4);
            var sedanDto2 = new SedanDto("AAAAAAAAAAAAAAAA3", "Toyota", "Corolla", 2019, 18000m, 4);

            _mapperMock.Setup(m => m.Map<IEnumerable<VehicleDto>>(It.IsAny<IEnumerable<Vehicle>>()))
                       .Returns<IEnumerable<Vehicle>>(v => v.Select(ve =>
                           ve.Id == "AAAAAAAAAAAAAAAAA" ? sedanDto :
                           ve.Id == "AAAAAAAAAAAAAAAA3" ? (VehicleDto)sedanDto2 : null!).Where(x => x != null));

            var service = CreateService();

            var result = await service.SearchAsync(type: VehicleType.Sedan, manufacturer: "Toyota");

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data?.Count());
            Assert.All(result.Data!, dto => Assert.Equal(VehicleType.Sedan, GetVehicleType(dto)));
        }

        private VehicleType GetVehicleType(VehicleDto dto)
        {
            return dto switch
            {
                SedanDto => VehicleType.Sedan,
                HatchbackDto => VehicleType.Hatchback,
                _ => throw new NotImplementedException()
            };
        }
    }

}
