using Auction.Management.Application.Dto;
using Auction.Management.Domain.Entities;
using AutoMapper;

namespace Auction.Management.Application.Profiles
{
    public class AuctionMappingProfile : Profile
    {
        public AuctionMappingProfile() {
            CreateMap<Domain.Entities.Auction, AuctionDto>()
                .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.Vehicle.Id))
                .ForMember(dest => dest.HighestBid, opt => opt.MapFrom(src => src.HighestBid));

            CreateMap<Bid, BidDto>()
                .ForMember(dest => dest.Bidder, opt => opt.MapFrom(src => src.Bidder));

            CreateMap<Bidder, BidderDto>();

            CreateMap<BidderDto, Bidder>();
        }
        
    }
}
