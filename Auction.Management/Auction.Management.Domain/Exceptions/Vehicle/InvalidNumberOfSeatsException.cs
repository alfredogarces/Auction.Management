namespace Auction.Management.Domain.Exceptions.Vehicle
{
    public class InvalidNumberOfSeatsException : Exception
    {
        public InvalidNumberOfSeatsException(int seats)
            : base($"Invalid number of seats: {seats}. Must be at least 1.")
        { }
    }
}
