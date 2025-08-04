using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Exceptions.Auction;
using Auction.Management.Domain.Validators;

namespace Auction.Management.Domain.Entities
{
    public class Auction
    {
        public Vehicle Vehicle { get; set; }
        public bool IsActive { get; private set; }
        public Bid? HighestBid { get; private set; }

        public Auction(Vehicle vehicle)
        {
            AuctionValidator.Validate(vehicle);
            Vehicle = vehicle;
            IsActive = false;
            HighestBid = null;
        }

        public void Start()
        {
            if (IsActive)
                throw new AuctionAlreadyStartedException();

            IsActive = true;
        }

        public void End()
        {
            if (!IsActive)
                throw new AuctionAlreadyEndedException();

            IsActive = false;
        }

        public void PlaceBid(Bid bid)
        {
            if (!IsActive)
                throw new AuctionNotActiveException();

            decimal minimum = HighestBid?.Amount ?? Vehicle.StartingBid;
            if (bid.Amount <= minimum)
                throw new BidTooLowException(bid.Amount, minimum);

            HighestBid = bid;
        }

        public Auction Clone()
        {
            var auction = new Domain.Entities.Auction(Vehicle.Clone());
            auction.IsActive = IsActive;
            auction.HighestBid = HighestBid?.Clone();
            return auction;
        }
    }
}
