using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Exceptions.Auction;
using Xunit;

namespace Auction.Management.Tests.UnitTests.Domain.Entities
{
    public class AuctionTests
    {
        private Vehicle CreateValidVehicle() =>
            new Truck("AAAAAAAAAAAAAAAAA", "Toyota", "Corolla", 2020, 10000, 5000);

        private Bidder CreateValidBidder() =>
            new Bidder("valid@email.com");

        [Fact]
        public void Constructor_ShouldCreateAuction_WhenVehicleIsValid()
        {
            var vehicle = CreateValidVehicle();
            var auction = new Management.Domain.Entities.Auction(vehicle);

            Assert.Equal(vehicle, auction.Vehicle);
            Assert.False(auction.IsActive);
            Assert.Null(auction.HighestBid);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenVehicleIsNull()
        {
            Assert.Throws<InvalidAuctionVehicleException>(() => new Management.Domain.Entities.Auction(null!));
        }

        [Fact]
        public void Start_ShouldActivateAuction_WhenNotStarted()
        {
            var auction = new Management.Domain.Entities.Auction(CreateValidVehicle());
            auction.Start();
            Assert.True(auction.IsActive);
        }

        [Fact]
        public void Start_ShouldThrow_WhenAuctionAlreadyStarted()
        {
            var auction = new Management.Domain.Entities.Auction(CreateValidVehicle());
            auction.Start();

            Assert.Throws<AuctionAlreadyStartedException>(() => auction.Start());
        }

        [Fact]
        public void End_ShouldDeactivateAuction_WhenActive()
        {
            var auction = new Management.Domain.Entities.Auction(CreateValidVehicle());
            auction.Start();
            auction.End();
            Assert.False(auction.IsActive);
        }

        [Fact]
        public void End_ShouldThrow_WhenAuctionNotStarted()
        {
            var auction = new Management.Domain.Entities.Auction(CreateValidVehicle());
            Assert.Throws<AuctionAlreadyEndedException>(() => auction.End());
        }

        [Fact]
        public void PlaceBid_ShouldSetHighestBid_WhenValidBid()
        {
            var auction = new Management.Domain.Entities.Auction(CreateValidVehicle());
            auction.Start();

            var bidder = CreateValidBidder();
            var bid = new Bid(bidder, 12000);

            auction.PlaceBid(bid);

            Assert.Equal(bid, auction.HighestBid);
        }

        [Fact]
        public void PlaceBid_ShouldThrow_WhenBidIsTooLow()
        {
            var auction = new Management.Domain.Entities.Auction(CreateValidVehicle());
            auction.Start();

            var bidder = CreateValidBidder();
            var lowBid = new Bid(bidder, 5000); // menor que StartingBid (10000)

            Assert.Throws<BidTooLowException>(() => auction.PlaceBid(lowBid));
        }

        [Fact]
        public void PlaceBid_ShouldThrow_WhenBidIsLowerThanPrevious()
        {
            var auction = new Management.Domain.Entities.Auction(CreateValidVehicle());
            auction.Start();

            var bidder1 = CreateValidBidder();
            var bidder2 = CreateValidBidder();

            var bid1 = new Bid(bidder1, 12000);
            var bid2 = new Bid(bidder2, 11500); // menor que bid1

            auction.PlaceBid(bid1);

            Assert.Throws<BidTooLowException>(() => auction.PlaceBid(bid2));
        }

        [Fact]
        public void PlaceBid_ShouldThrow_WhenAuctionNotStarted()
        {
            var auction = new Management.Domain.Entities.Auction(CreateValidVehicle());
            var bid = new Bid(CreateValidBidder(), 11000);

            Assert.Throws<AuctionNotActiveException>(() => auction.PlaceBid(bid));
        }
    }
}
