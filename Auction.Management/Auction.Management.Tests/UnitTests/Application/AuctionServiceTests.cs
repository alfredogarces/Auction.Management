using Auction.Management.Application.Common;
using Auction.Management.Application.Dto;
using Auction.Management.Application.Interfaces;
using Auction.Management.Application.Services;
using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Interfaces;
using AutoMapper;
using Moq;

namespace Auction.Management.Tests.UnitTests.Application
{
    public class AuctionServiceTests
    {
        private readonly Mock<IAuctionRepository> _auctionRepoMock = new();
        private readonly Mock<IVehicleRepository> _vehicleRepoMock = new();
        private readonly Mock<IBidderService> _bidderServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private AuctionService CreateService() =>
            new(_auctionRepoMock.Object, _vehicleRepoMock.Object, _bidderServiceMock.Object, _mapperMock.Object);

        [Fact]
        public async Task StartAuctionAsync_ShouldStartAuction_WhenVehicleExistsAndAuctionNotStarted()
        {
            var vehicleId = "AAAAAAAAAAAAAAAAA";
            var vehicle = new Sedan(vehicleId, "Toyota", "Camry", 2022, 25000m, 4);

            _vehicleRepoMock.Setup(r => r.GetByIdAsync(vehicleId)).ReturnsAsync(vehicle);
            _auctionRepoMock.Setup(r => r.GetByVehicleId(vehicleId)).ReturnsAsync((Management.Domain.Entities.Auction?)null);
            _auctionRepoMock.Setup(r => r.Add(It.IsAny<Management.Domain.Entities.Auction>())).Returns(Task.CompletedTask);
            _auctionRepoMock.Setup(r => r.Update(It.IsAny<Management.Domain.Entities.Auction>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<AuctionDto>(It.IsAny<Management.Domain.Entities.Auction>()))
                       .Returns(new AuctionDto(vehicleId, false, new BidDto(new BidderDto("valid@email.com"), 1000)));

            var service = CreateService();

            var result = await service.StartAuctionAsync(vehicleId);

            Assert.True(result.IsSuccess);
            Assert.Equal(vehicleId, result.Data?.VehicleId);
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldReturnFailure_WhenAuctionAlreadyStarted()
        {
            var vehicleId = "AAAAAAAAAAAAAAAAA";
            var vehicle = new Sedan(vehicleId, "Toyota", "Camry", 2022, 25000m, 4);
            var auction = new Management.Domain.Entities.Auction(vehicle);
            auction.Start();

            _vehicleRepoMock.Setup(r => r.GetByIdAsync(vehicleId)).ReturnsAsync(vehicle);
            _auctionRepoMock.Setup(r => r.GetByVehicleId(vehicleId)).ReturnsAsync(auction);

            var service = CreateService();

            var result = await service.StartAuctionAsync(vehicleId);

            Assert.True(result.IsFailure);
            Assert.Equal("Auction has already started.", result.Errors?.FirstOrDefault()?.Description);
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldPlaceBid_WhenBidIsValid()
        {
            var vehicleId = "AAAAAAAAAAAAAAAAA";
            var vehicle = new Sedan(vehicleId, "Toyota", "Camry", 2022, 25000m, 4);
            var auction = new Management.Domain.Entities.Auction(vehicle);
            auction.Start();

            var bidderDto = new BidderDto("john@example.com");
            var bidDto = new BidDto(bidderDto, 30000m);

            var bidder = new Bidder("john@example.com");

            _auctionRepoMock.Setup(r => r.GetByVehicleId(vehicleId)).ReturnsAsync(auction);
            _bidderServiceMock.Setup(b => b.GetByEmailAsync("john@example.com"))
                              .ReturnsAsync(Result<BidderDto>.Failure(new Error("not found")));
            _bidderServiceMock.Setup(b => b.AddBidderAsync(It.IsAny<BidderDto>()))
                              .ReturnsAsync(Result<BidderDto>.Success(bidderDto));
            _mapperMock.Setup(m => m.Map<Bidder>(bidderDto)).Returns(bidder);
            _auctionRepoMock.Setup(r => r.Update(It.IsAny<Management.Domain.Entities.Auction>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<AuctionDto>(auction))
                       .Returns(new AuctionDto(vehicleId, true, new BidDto(new BidderDto("valid@email.com"), 30000)));

            var service = CreateService();

            var result = await service.PlaceBidAsync(vehicleId, bidDto);

            Assert.True(result.IsSuccess);
            Assert.Equal(vehicleId, result.Data?.VehicleId);
            Assert.Equal(bidDto.Amount, result.Data?.HighestBid?.Amount);
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldReturnFailure_WhenBidTooLow()
        {
            var vehicleId = "AAAAAAAAAAAAAAAAA";
            var vehicle = new Sedan(vehicleId, "Toyota", "Camry", 2022, 25000m, 4);
            var auction = new Management.Domain.Entities.Auction(vehicle);
            auction.Start();

            var bidderDto = new BidderDto("john@example.com");
            var bidDto = new BidDto(bidderDto, 25000m);

            var bidder = new Bidder("john@example.com");

            _auctionRepoMock.Setup(r => r.GetByVehicleId(vehicleId)).ReturnsAsync(auction);
            _bidderServiceMock.Setup(b => b.GetByEmailAsync("john@example.com"))
                              .ReturnsAsync(Result<BidderDto>.Success(bidderDto));
            _mapperMock.Setup(m => m.Map<Bidder>(bidderDto)).Returns(bidder);

            var service = CreateService();

            var result = await service.PlaceBidAsync(vehicleId, bidDto);

            Assert.True(result.IsFailure);
            Assert.Equal("Bid amount '25000' is too low. Minimum required is '25000'.", result.Errors?.FirstOrDefault()?.Description);
        }

        [Fact]
        public async Task CloseAuctionAsync_ShouldCloseAuction_WhenAuctionIsActive()
        {
            var vehicleId = "AAAAAAAAAAAAAAAAA";
            var vehicle = new Sedan(vehicleId, "Toyota", "Camry", 2022, 25000m, 4);
            var auction = new Management.Domain.Entities.Auction(vehicle);
            auction.Start();

            _auctionRepoMock.Setup(r => r.GetByVehicleId(vehicleId)).ReturnsAsync(auction);
            _auctionRepoMock.Setup(r => r.Update(auction)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<AuctionDto>(auction))
                       .Returns(new AuctionDto(vehicleId, false, new BidDto(new BidderDto("valid@email.com"), 1000)));

            var service = CreateService();

            var result = await service.CloseAuctionAsync(vehicleId);

            Assert.True(result.IsSuccess);
            Assert.Equal(vehicleId, result.Data?.VehicleId);
        }

        [Fact]
        public async Task CloseAuctionAsync_ShouldReturnFailure_WhenAuctionAlreadyEnded()
        {
            var vehicleId = "AAAAAAAAAAAAAAAAA";
            var vehicle = new Sedan(vehicleId, "Toyota", "Camry", 2022, 25000m, 4);
            var auction = new Management.Domain.Entities.Auction(vehicle);

            _auctionRepoMock.Setup(r => r.GetByVehicleId(vehicleId)).ReturnsAsync(auction);

            var service = CreateService();

            var result = await service.CloseAuctionAsync(vehicleId);

            Assert.True(result.IsFailure);
            Assert.Equal("Auction has already ended.", result.Errors?.FirstOrDefault()?.Description);
        }
    }
}
