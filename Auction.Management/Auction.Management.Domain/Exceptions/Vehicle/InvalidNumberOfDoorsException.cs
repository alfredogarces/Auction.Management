namespace Auction.Management.Domain.Exceptions.Vehicle
{
    public class InvalidNumberOfDoorsException : Exception
    {
        public InvalidNumberOfDoorsException(int doors)
            : base($"Invalid number of doors: {doors}. Must be at least 2.")
        { }
    }
}
