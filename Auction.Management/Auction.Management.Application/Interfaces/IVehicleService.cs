using Auction.Management.Application.Common;
using Auction.Management.Application.Dto;
using Auction.Management.Domain.Enums;

namespace Auction.Management.Application.Interfaces
{
    public interface IVehicleService
    {
        Task<Result<VehicleDto>> AddVehicle(VehicleDto vehicleDto);
        Task<Result<IEnumerable<VehicleDto>>> SearchAsync(VehicleType? type = null,
                                                                        string? manufacturer = null,
                                                                        string? model = null,
                                                                        int? year = null);

    }
}
