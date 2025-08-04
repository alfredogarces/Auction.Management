using Auction.Management.Application.Dto;
using FluentAssertions;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;

namespace Auction.Management.Tests.IntegrationTest
{
    public class VehicleControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public VehicleControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddSUV_ShouldReturnCreated_WhenValid()
        {
            var suvDto = new
            {
                vehicleType = "SUV",
                id = "AAAAAAAAAAAAAAAAY",
                manufacturer = "Ford",
                model = "Explorer",
                year = 2021,
                startingBid = 20000,
                numberOfSeats = 7
            };

            var response = await _client.PostAsJsonAsync("/api/vehicle", suvDto);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var responseData = await response.Content.ReadFromJsonAsync<SuvDto>();
            responseData.Should().NotBeNull();
            responseData.Id.Should().Be(suvDto.id);
        }

        [Fact]
        public async Task AddTruck_ShouldReturnCreated_WhenValid()
        {
            var truckDto = new
            {
                vehicleType = "Truck",
                id = "AAAAAAAAAAAAAAAAR",
                manufacturer = "Volvo",
                model = "FH16",
                year = 2020,
                startingBid = 80000,
                loadCapacity = 25.5
            };

            var response = await _client.PostAsJsonAsync("/api/vehicle", truckDto);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseData = await response.Content.ReadFromJsonAsync<TruckDto>();
            responseData.Should().NotBeNull();
            responseData!.Id.Should().Be(truckDto.id);
        }

        [Fact]
        public async Task AddSedan_ShouldReturnCreated_WhenValid()
        {
            var sedanDto = new
            {
                vehicleType = "Sedan",
                id = "AAAAAAAAAAAAAAAAS",
                manufacturer = "Toyota",
                model = "Camry",
                year = 2022,
                startingBid = 30000,
                numberOfDoors = 4
            };

            var response = await _client.PostAsJsonAsync("/api/vehicle", sedanDto);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseData = await response.Content.ReadFromJsonAsync<SedanDto>();
            responseData.Should().NotBeNull();
            responseData!.Id.Should().Be(sedanDto.id);
        }

        [Fact]
        public async Task AddHatchback_ShouldReturnCreated_WhenValid()
        {
            var hatchbackDto = new
            {
                vehicleType = "Hatchback",
                id = "AAAAAAAAAAAAAAAAT",
                manufacturer = "Volkswagen",
                model = "Golf",
                year = 2019,
                startingBid = 18000,
                numberOfDoors = 4
            };

            var response = await _client.PostAsJsonAsync("/api/vehicle", hatchbackDto);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseData = await response.Content.ReadFromJsonAsync<HatchbackDto>();
            responseData.Should().NotBeNull();
            responseData!.Id.Should().Be(hatchbackDto.id);
        }

        [Fact]
        public async Task GetVehicles_ShouldReturnPolymorphicVehicleDtos()
        {
            var response = await _client.GetAsync("api/vehicle");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var rawJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };

            var vehicles = JsonSerializer.Deserialize<List<VehicleDto>>(rawJson, options);

            vehicles.Should().NotBeNull();
            vehicles.Should().NotBeEmpty();

            vehicles.Any(v => v is SuvDto).Should().BeTrue();
            vehicles.Any(v => v is TruckDto).Should().BeTrue();
            vehicles.Any(v => v is SedanDto).Should().BeTrue();
            vehicles.Any(v => v is HatchbackDto).Should().BeTrue();
        }

        [Fact]
        public async Task GetVehicles_FilterByType_ShouldReturnOnlySpecifiedType()
        {
            var response = await _client.GetAsync("api/vehicle?type=SUV");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var vehicles = JsonSerializer.Deserialize<List<VehicleDto>>(json, options);

            vehicles.Should().NotBeNull();
            vehicles.Should().OnlyContain(v => v is SuvDto);

            vehicles.Should().ContainSingle(v => v.Id == "BBBBBBBBBBBBBBBBB");
        }

        [Fact]
        public async Task GetVehicles_FilterByManufacturer_ShouldReturnOnlySpecifiedManufacturer()
        {
            var response = await _client.GetAsync("api/vehicle?manufacturer=Toyota");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var vehicles = JsonSerializer.Deserialize<List<VehicleDto>>(json, options);

            vehicles.Should().NotBeNull();
            vehicles.Should().OnlyContain(v => v.Manufacturer.Equals("Toyota", StringComparison.OrdinalIgnoreCase));

            vehicles.Should().ContainSingle(v => v.Id == "AAAAAAAAAAAAAAAAA");
        }

        [Fact]
        public async Task GetVehicles_FilterByModel_ShouldReturnOnlySpecifiedModel()
        {
            var response = await _client.GetAsync("api/vehicle?model=Golf");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var vehicles = JsonSerializer.Deserialize<List<VehicleDto>>(json, options);

            vehicles.Should().NotBeNull();
            vehicles.Should().OnlyContain(v => v.Model.Equals("Golf", StringComparison.OrdinalIgnoreCase));

            vehicles.Should().ContainSingle(v => v.Id == "DDDDDDDDDDDDDDDDD");
        }

        [Fact]
        public async Task GetVehicles_FilterByYear_ShouldReturnOnlySpecifiedYear()
        {
            var response = await _client.GetAsync("api/vehicle?year=2021");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var vehicles = JsonSerializer.Deserialize<List<VehicleDto>>(json, options);

            vehicles.Should().NotBeNull();

            vehicles.Should().OnlyContain(v => v.Year == 2021);
            vehicles.Should().ContainSingle(v => v.Id == "BBBBBBBBBBBBBBBBB");
        }

        [Fact]
        public async Task StartAuction_ShouldReturnNotFound_WhenVehicleDoesNotExist()
        {
            var invalidVehicleId = "INVALIDID123456789";

            var response = await _client.PostAsync($"api/auction/{invalidVehicleId}/start", null);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


    }
}