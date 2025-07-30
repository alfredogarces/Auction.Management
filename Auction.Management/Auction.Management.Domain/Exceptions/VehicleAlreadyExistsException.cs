namespace Auction.Management.Domain.Exceptions
{
    public class VehicleAlreadyExistsException : Exception
    {
        public VehicleAlreadyExistsException(string id)
            : base($"Vehicle with ID '{id}' already exists in the inventory.") { }
    }
}
