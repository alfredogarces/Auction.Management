using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Interfaces;

namespace Auction.Management.Infrastructure.InMemoryRepositories
{
    public class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles = new List<Vehicle>
            {
                new Sedan("AAAAAAAAAAAAAAAAA", "Toyota", "Camry", 2020, 15000m, 4),
                new SUV("BBBBBBBBBBBBBBBBB", "Ford", "Explorer", 2021, 20000m, 7),
                new Truck("CCCCCCCCCCCCCCCCC", "Volvo", "FH16", 2019, 50000m, 25000),
                new Hatchback("DDDDDDDDDDDDDDDDD", "Volkswagen", "Golf", 2018, 12000m, 5)
            };

        public Task AddAsync(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
            return Task.CompletedTask;
        }

        public Task<Vehicle?> GetByIdAsync(string id)
        {
            var vehicle = _vehicles.FirstOrDefault(v => v.Id == id);
            return Task.FromResult(vehicle);
        }

        public Task<bool> ExistsAsync(string id)
        {
            var exists = _vehicles.Any(v => v.Id == id);
            return Task.FromResult(exists);
        }

        public Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Vehicle>>(_vehicles.ToList());
        }
    }
}
