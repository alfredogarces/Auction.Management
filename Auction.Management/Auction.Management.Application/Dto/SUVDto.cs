namespace Auction.Management.Application.Dto
{
    public record SuvDto(
        string Id,
        string Manufacturer,
        string Model,
        int Year,
        decimal StartingBid,
        int NumberOfSeats
    ) : VehicleDto(Id, Manufacturer, Model, Year, StartingBid);

}
