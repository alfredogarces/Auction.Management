using Auction.Management.Application.Common;
using Auction.Management.Application.Dto;
using Auction.Management.Application.Interfaces;
using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Enums;
using Auction.Management.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Auction.Management.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleService> _logger;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


        public VehicleService(
            IVehicleRepository vehicleRepository,
            IMapper mapper,
            ILogger<VehicleService> logger)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Result<VehicleDto>> AddVehicle(VehicleDto vehicleDto)
        {
            _logger.LogInformation("Adding new vehicle with ID {VehicleId}", vehicleDto.Id);

            try
            {
                var vehicle = _mapper.Map<Vehicle>(vehicleDto);

                var added = await _vehicleRepository.AddAsync(vehicle);

                if (!added)
                {
                    _logger.LogWarning("Vehicle with ID {VehicleId} already exists", vehicleDto.Id);
                    return Result<VehicleDto>.Failure(new Error($"Vehicle with ID {vehicleDto.Id} already exists."));
                }

                _logger.LogInformation("Vehicle with ID {VehicleId} added successfully", vehicleDto.Id);

                return Result<VehicleDto>.Success(vehicleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding vehicle with ID {VehicleId}", vehicleDto.Id);
                return Result<VehicleDto>.Failure(new Error(ex.Message));
            }
        }


        public async Task<Result<IEnumerable<VehicleDto>>> SearchAsync(VehicleType? type = null,
                                                                        string? manufacturer = null,
                                                                        string? model = null,
                                                                        int? year = null)
        {
            _logger.LogInformation("Searching for vehicles with filters: Type={Type}, Manufacturer={Manufacturer}, Model={Model}, Year={Year}",
                type, manufacturer, model, year);

            try
            {
                var allVehicles = await _vehicleRepository.GetAllAsync();

                var filtered = allVehicles.Where(v =>
                    (type == null || v.GetVehicleType() == type) &&
                    (manufacturer == null || v.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase)) &&
                    (model == null || v.Model.Equals(model, StringComparison.OrdinalIgnoreCase)) &&
                    (year == null || v.Year == year)
                );

                var dtos = _mapper.Map<IEnumerable<VehicleDto>>(filtered);

                _logger.LogInformation("{Count} vehicles found with applied filters", dtos.Count());

                return Result<IEnumerable<VehicleDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for vehicles");
                return Result<IEnumerable<VehicleDto>>.Failure(new Error(ex.Message));
            }
        }
    }
}
