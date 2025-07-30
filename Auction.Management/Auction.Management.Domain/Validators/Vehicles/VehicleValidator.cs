using Auction.Management.Domain.Exceptions.Vehicle;
using System.Text.RegularExpressions;

namespace Auction.Management.Domain.Validators.Vehicles
{
    public static class VehicleValidator
    {
        public static void Validate(string id, string manufacturer, string model, int year, decimal startingBid)
        {
            if (!IsValidVin(id))
                throw new InvalidVehicleIdException();

            if (string.IsNullOrWhiteSpace(manufacturer))
                throw new InvalidManufacturerException();

            if (string.IsNullOrWhiteSpace(model))
                throw new InvalidModelException();

            var currentYear = DateTime.Now.Year;
            if (year < 1886 || year > currentYear + 1)
                throw new InvalidYearException(year);

            if (startingBid < 0)
                throw new InvalidStartingBidException(startingBid);
        }

        private static bool IsValidVin(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                return false;

            return Regex.IsMatch(vin, "^[A-HJ-NPR-Z0-9]{17}$", RegexOptions.IgnoreCase);
        }
    }
}
