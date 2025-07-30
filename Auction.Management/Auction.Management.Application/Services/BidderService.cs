using Auction.Management.Application.Common;
using Auction.Management.Application.Dto;
using Auction.Management.Application.Interfaces;
using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Interfaces;
using AutoMapper;

namespace Auction.Management.Application.Services
{
    public class BidderService : IBidderService
    {
        private readonly IBidderRepository _bidderRepository;
        private readonly IMapper _mapper;

        public BidderService(IBidderRepository bidderRepository, IMapper mapper)
        {
            _bidderRepository = bidderRepository;
            _mapper = mapper;
        }

        public async Task<Result<BidderDto>> GetByEmailAsync(string email)
        {
            var bidder = await _bidderRepository.GetByEmailAsync(email);
            if (bidder == null)
                return Result<BidderDto>.Failure(new Error("Bidder not found"));

            var bidderDto = _mapper.Map<BidderDto>(bidder);
            return Result<BidderDto>.Success(bidderDto);
        }

        public async Task<Result<BidderDto>> AddBidderAsync(BidderDto bidderDto)
        {
            var bidder = _mapper.Map<Bidder>(bidderDto);

            var existing = await _bidderRepository.GetByEmailAsync(bidder.Email);
            if (existing != null)
                return Result<BidderDto>.Failure(new Error("Bidder already exists."));

            await _bidderRepository.AddAsync(bidder);

            var newBidderDto = _mapper.Map<BidderDto>(bidder);
            return Result<BidderDto>.Success(newBidderDto);
        }
    }

}