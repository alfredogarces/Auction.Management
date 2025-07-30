using Auction.Management.Application.Dto;
using Auction.Management.Domain.Entities.Vehicles;
using AutoMapper;

namespace Auction.Management.Application.Profiles
{
    public class VehicleMappingProfile : Profile
    {
        public VehicleMappingProfile()
        {
            CreateMap<Vehicle, VehicleDto>()
                .Include<Truck, TruckDto>()
                .Include<SUV, SuvDto>()
                .Include<Sedan, SedanDto>()
                .Include<Hatchback, HatchbackDto>();

            CreateMap<Truck, TruckDto>();
            CreateMap<SUV, SuvDto>();
            CreateMap<Sedan, SedanDto>();
            CreateMap<Hatchback, HatchbackDto>();

            CreateMap<VehicleDto, Vehicle>()
                .Include<SuvDto, SUV>()
                .Include<TruckDto, Truck>()
                .Include<SedanDto, Sedan>()
                .Include<HatchbackDto, Hatchback>();

            CreateMap<SuvDto, SUV>();
            CreateMap<TruckDto, Truck>();
            CreateMap<SedanDto, Sedan>();
            CreateMap<HatchbackDto, Hatchback>();
        }
    }

}