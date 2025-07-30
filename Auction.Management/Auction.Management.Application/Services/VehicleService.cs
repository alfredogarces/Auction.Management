using Auction.Management.Application.Common;
using Auction.Management.Application.Dto;
using Auction.Management.Application.Interfaces;
using Auction.Management.Domain.Entities.Vehicles;
using Auction.Management.Domain.Enums;
using Auction.Management.Domain.Interfaces;
using AutoMapper;

namespace Auction.Management.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            this._vehicleRepository = vehicleRepository;
            this._mapper = mapper;
        }

        public async Task<Result<VehicleDto>> AddVehicle(VehicleDto vehicleDto)
        {
            try
            {
                Vehicle vehicle = _mapper.Map<Vehicle>(vehicleDto);

                if (await _vehicleRepository.GetByIdAsync(vehicle.Id) != null)
                    return Result<VehicleDto>.Failure(new Error($"Vehicle with ID {vehicle.Id} already exists."));

                await _vehicleRepository.AddAsync(vehicle);

                var resultDto = _mapper.Map<VehicleDto>(vehicle);
                return Result<VehicleDto>.Success(resultDto);
            }
            catch (Exception ex)
            {
                return Result<VehicleDto>.Failure(new Error(ex.Message));
            }
        }

        public async Task<Result<IEnumerable<VehicleDto>>> SearchAsync(VehicleType? type = null,
                                                                        string? manufacturer = null,
                                                                        string? model = null, 
                                                                        int? year = null)
        {
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

                return Result<IEnumerable<VehicleDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleDto>>.Failure(new Error(ex.Message));
            }
        }

    }

}
