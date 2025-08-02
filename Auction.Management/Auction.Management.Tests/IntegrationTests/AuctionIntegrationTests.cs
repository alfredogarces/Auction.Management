using Auction.Management.Application.Dto;
using FluentAssertions;
using System.Net.Http.Json;
using System.Net;

public class AuctionControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly string vehicleIdForStart = "AAAAAAAAAAAAAAAAA";
    private readonly string vehicleIdForBid = "BBBBBBBBBBBBBBBBB";
    private readonly string vehicleIdForClose = "CCCCCCCCCCCCCCCCC";

    public AuctionControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task EnsureAuctionStarted(string vehicleId)
    {
        var response = await _client.PostAsync($"api/auction/{vehicleId}/start", null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task StartAuction_ShouldReturnOk_WhenVehicleExists()
    {
        var response = await _client.PostAsync($"api/auction/{vehicleIdForStart}/start", null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var auctionDto = await response.Content.ReadFromJsonAsync<AuctionDto>();
        auctionDto.Should().NotBeNull();
        auctionDto.VehicleId.Should().Be(vehicleIdForStart);
        auctionDto.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task PlaceBid_ShouldReturnOk_WhenAuctionExistsAndBidIsValid()
    {
        await EnsureAuctionStarted(vehicleIdForBid);

        var bidDto = new BidDto(new BidderDto("john.doe@example.com"), 25000m);

        var response = await _client.PostAsJsonAsync($"api/auction/{vehicleIdForBid}/bid", bidDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var auctionDto = await response.Content.ReadFromJsonAsync<AuctionDto>();
        auctionDto.Should().NotBeNull();
        auctionDto.VehicleId.Should().Be(vehicleIdForBid);
        auctionDto.HighestBid!.Amount.Should().Be(25000m);
    }

    [Fact]
    public async Task CloseAuction_ShouldReturnOk_WhenAuctionExists()
    {
        await EnsureAuctionStarted(vehicleIdForClose);

        var response = await _client.PostAsync($"api/auction/{vehicleIdForClose}/close", null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var auctionDto = await response.Content.ReadFromJsonAsync<AuctionDto>();
        auctionDto.Should().NotBeNull();
        auctionDto.VehicleId.Should().Be(vehicleIdForClose);
        auctionDto.IsActive.Should().BeFalse();
    }
}
