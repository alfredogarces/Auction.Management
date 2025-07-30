namespace Auction.Management.Domain.Exceptions.Auction
{
    public class InvalidAuctionVehicleException : ArgumentException
    {
        public InvalidAuctionVehicleException()
            : base("Vehicle for auction cannot be null.") { }
    }
}
