namespace Auction.Management.Domain.Exceptions
{
    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException(string id)
            : base($"Vehicle with ID '{id}' was not found.")
        {
        }
    }
}
