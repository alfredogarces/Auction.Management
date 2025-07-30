using Auction.Management.Domain.Entities.Vehicles;

namespace Auction.Management.Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task AddAsync(Vehicle vehicle);
        Task<Vehicle?> GetByIdAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
    }
}
