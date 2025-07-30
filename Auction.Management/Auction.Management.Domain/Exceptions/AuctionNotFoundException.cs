namespace Auction.Management.Domain.Exceptions
{
    public class AuctionNotFoundException : Exception
    {
        public AuctionNotFoundException(string vehicleId)
            : base($"No auction was found for vehicle with ID '{vehicleId}'.") { }
    }
}
