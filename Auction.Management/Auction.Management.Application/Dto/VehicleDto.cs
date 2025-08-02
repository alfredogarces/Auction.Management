using System.Text.Json.Serialization;

namespace Auction.Management.Application.Dto
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "vehicleType")]
    [JsonDerivedType(typeof(TruckDto), "Truck")]
    [JsonDerivedType(typeof(SuvDto), "SUV")]
    [JsonDerivedType(typeof(SedanDto), "Sedan")]
    [JsonDerivedType(typeof(HatchbackDto), "Hatchback")]
    public abstract record VehicleDto(string Id, string Manufacturer, string Model, int Year, decimal StartingBid);
}
