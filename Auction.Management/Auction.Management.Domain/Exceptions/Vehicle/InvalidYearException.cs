namespace Auction.Management.Domain.Exceptions.Vehicle
{
    public class InvalidYearException : ArgumentException
    {
        public InvalidYearException(int year)
            : base($"Year '{year}' is invalid. It must be between 1886 and {DateTime.Now.Year + 1}.")
        {
        }
    }
}
