using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Interfaces;
using System.Collections.Concurrent;

namespace Auction.Management.Infrastructure.InMemoryRepositories
{
    public class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly ConcurrentDictionary<string, Vehicle> _vehicles = new()
        {
            ["AAAAAAAAAAAAAAAAA"] = new Sedan("AAAAAAAAAAAAAAAAA", "Toyota", "Camry", 2020, 15000m, 4),
            ["BBBBBBBBBBBBBBBBB"] = new SUV("BBBBBBBBBBBBBBBBB", "Ford", "Explorer", 2021, 20000m, 7),
            ["CCCCCCCCCCCCCCCCC"] = new Truck("CCCCCCCCCCCCCCCCC", "Volvo", "FH16", 2019, 50000m, 25000),
            ["DDDDDDDDDDDDDDDDD"] = new Hatchback("DDDDDDDDDDDDDDDDD", "Volkswagen", "Golf", 2018, 12000m, 5)
        };

        public Task<bool> AddAsync(Vehicle vehicle)
        {
            var added = _vehicles.TryAdd(vehicle.Id, vehicle.Clone());
            return Task.FromResult(added);
        }

        public Task<Vehicle?> GetByIdAsync(string id)
        {
            _vehicles.TryGetValue(id, out var vehicle);
            return Task.FromResult(vehicle?.Clone());
        }

        public Task<bool> ExistsAsync(string id)
        {
            var exists = _vehicles.ContainsKey(id);
            return Task.FromResult(exists);
        }

        public Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            var clones = _vehicles.Values.Select(v => v.Clone()).ToList();
            return Task.FromResult<IEnumerable<Vehicle>>(clones);
        }
    }
}
