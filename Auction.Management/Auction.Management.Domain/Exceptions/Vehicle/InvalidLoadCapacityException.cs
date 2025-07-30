namespace Auction.Management.Domain.Exceptions.Vehicle
{
    public class InvalidLoadCapacityException : Exception
    {
        public InvalidLoadCapacityException(double capacity)
            : base($"Invalid load capacity: {capacity}. Must be positive.")
        { }
    }
}
