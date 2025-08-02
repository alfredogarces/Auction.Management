using Auction.Management.Application.Common;
using Auction.Management.Application.Dto;
using Auction.Management.Application.Interfaces;
using Auction.Management.Application.Validators;
using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Interfaces;
using AutoMapper;

namespace Auction.Management.Application.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IBidderService _bidderService;
        private readonly IMapper _mapper;

        private static readonly Dictionary<string, SemaphoreSlim> _auctionLocks = new();
        private static readonly object _lockDictSync = new();

        public AuctionService(
            IAuctionRepository auctionRepository,
            IVehicleRepository vehicleRepository,
            IBidderService bidderService,
            IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _vehicleRepository = vehicleRepository;
            _bidderService = bidderService;
            _mapper = mapper;
        }

        public async Task<Result<AuctionDto>> StartAuctionAsync(string vehicleId)
        {
            var semaphore = GetAuctionLock(vehicleId);
            await semaphore.WaitAsync();

            try
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
                if (vehicle == null)
                    return Result<AuctionDto>.Failure(new Error($"Vehicle with ID {vehicleId} not found."));

                var existingAuction = await _auctionRepository.GetByVehicleId(vehicleId);
                AuctionServiceValidator.ValidateStartAuction(existingAuction);

                var auction = existingAuction ?? new Domain.Entities.Auction(vehicle);
                if (existingAuction == null)
                    await _auctionRepository.Add(auction);

                auction.Start();
                await _auctionRepository.Update(auction);

                var auctionDto = _mapper.Map<AuctionDto>(auction);
                return Result<AuctionDto>.Success(auctionDto);
            }
            catch (Exception ex)
            {
                return Result<AuctionDto>.Failure(new Error(ex.Message));
            }
            finally
            {
                semaphore.Release();
            }
        }


        public async Task<Result<AuctionDto>> PlaceBidAsync(string vehicleId, BidDto bidDto)
        {
            var semaphore = GetAuctionLock(vehicleId);
            await semaphore.WaitAsync();

            try
            {
                var auction = await _auctionRepository.GetByVehicleId(vehicleId);
                if (auction == null)
                    return Result<AuctionDto>.Failure(new Error($"There is no Auction for this vehicle {vehicleId}."));

                var bidderResult = await GetOrCreateBidderAsync(bidDto.Bidder);
                if (bidderResult.IsFailure)
                    return Result<AuctionDto>.Failure(new Error("Failed to find or create bidder."));

                var bid = new Bid(bidderResult.Data!, bidDto.Amount);
                auction.PlaceBid(bid);
                await _auctionRepository.Update(auction);

                var auctionDto = _mapper.Map<AuctionDto>(auction);
                return Result<AuctionDto>.Success(auctionDto);
            }
            catch (Exception ex)
            {
                return Result<AuctionDto>.Failure(new Error(ex.Message));
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task<Result<Bidder>> GetOrCreateBidderAsync(BidderDto bidderDto)
        {
            var existingBidderResult = await _bidderService.GetByEmailAsync(bidderDto.Email);
            if (existingBidderResult.IsSuccess)
            {
                var bidder = _mapper.Map<Bidder>(existingBidderResult.Data);
                return Result<Bidder>.Success(bidder);
            }

            var addBidderResult = await _bidderService.AddBidderAsync(bidderDto);
            if (addBidderResult.IsFailure)
                return Result<Bidder>.Failure(new Error("Could not create bidder."));

            var newBidder = _mapper.Map<Bidder>(addBidderResult.Data);
            return Result<Bidder>.Success(newBidder);
        }

        public async Task<Result<AuctionDto>> CloseAuctionAsync(string vehicleId)
        {
            var semaphore = GetAuctionLock(vehicleId);
            await semaphore.WaitAsync();

            try
            {
                var auction = await _auctionRepository.GetByVehicleId(vehicleId);
                if (auction == null)
                    return Result<AuctionDto>.Failure(new Error($"There is no Auction for this vehicle {vehicleId}."));

                auction.End();
                await _auctionRepository.Update(auction);

                var auctionDto = _mapper.Map<AuctionDto>(auction);
                return Result<AuctionDto>.Success(auctionDto);
            }
            catch (Exception ex)
            {
                return Result<AuctionDto>.Failure(new Error(ex.Message));
            }
            finally
            {
                semaphore.Release();
            }
        }


        private static SemaphoreSlim GetAuctionLock(string vehicleId)
        {
            lock (_lockDictSync)
            {
                if (!_auctionLocks.TryGetValue(vehicleId, out var semaphore))
                {
                    semaphore = new SemaphoreSlim(1, 1);
                    _auctionLocks[vehicleId] = semaphore;
                }
                return semaphore;
            }
        }


    }
}
