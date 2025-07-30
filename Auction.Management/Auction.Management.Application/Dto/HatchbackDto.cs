namespace Auction.Management.Application.Dto
{
    public record HatchbackDto(
    string Id,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid,
    int NumberOfDoors
) : VehicleDto(Id, Manufacturer, Model, Year, StartingBid);
}
