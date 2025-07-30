namespace Auction.Management.Domain.Interfaces
{
    public interface IAuctionRepository
    {
        Task Add(Entities.Auction auction);
        Task<Entities.Auction?> GetByVehicleId(string vehicleId);
        Task<IEnumerable<Entities.Auction>> GetAll();
        Task Update(Entities.Auction auction);
    }

}
