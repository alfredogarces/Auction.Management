    namespace Auction.Management.Application.Dto
{
    public abstract record VehicleDto(string Id, string Manufacturer, string Model, int Year, decimal StartingBid);
}
