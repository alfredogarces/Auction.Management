namespace Auction.Management.Application.Dto
{
    public record TruckDto(
        string Id,
        string Manufacturer,
        string Model,
        int Year,
        decimal StartingBid,
        double LoadCapacity
    ) : VehicleDto(Id, Manufacturer, Model, Year, StartingBid);
}
