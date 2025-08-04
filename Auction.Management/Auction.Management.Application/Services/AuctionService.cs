using Auction.Management.Application.Common;
using Auction.Management.Application.Dto;
using Auction.Management.Application.Interfaces;
using Auction.Management.Application.Validators;
using Auction.Management.Domain.Entities;
using Auction.Management.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Auction.Management.Application.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IBidderService _bidderService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuctionService> _logger;

        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _auctionLocks = new();

        public AuctionService(
            IAuctionRepository auctionRepository,
            IVehicleRepository vehicleRepository,
            IBidderService bidderService,
            IMapper mapper,
            ILogger<AuctionService> logger)
        {
            _auctionRepository = auctionRepository;
            _vehicleRepository = vehicleRepository;
            _bidderService = bidderService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<AuctionDto>> StartAuctionAsync(string vehicleId)
        {
            var semaphore = GetAuctionLock(vehicleId);
            await semaphore.WaitAsync();

            _logger.LogInformation("Starting auction for vehicle {VehicleId}", vehicleId);

            try
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
                if (vehicle == null)
                {
                    _logger.LogWarning("Vehicle not found with ID: {VehicleId}", vehicleId);
                    return Result<AuctionDto>.Failure(new Error($"Vehicle with ID {vehicleId} not found."));
                }

                var existingAuction = await _auctionRepository.GetByVehicleId(vehicleId);
                AuctionServiceValidator.ValidateStartAuction(existingAuction);

                var auction = existingAuction ?? new Domain.Entities.Auction(vehicle);
                if (existingAuction == null)
                {
                    _logger.LogInformation("Creating new auction for vehicle {VehicleId}", vehicleId);
                    await _auctionRepository.Add(auction);
                }

                auction.Start();
                await _auctionRepository.Update(auction);

                _logger.LogInformation("Auction started for vehicle {VehicleId}", vehicleId);

                var auctionDto = _mapper.Map<AuctionDto>(auction);
                return Result<AuctionDto>.Success(auctionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting auction for vehicle {VehicleId}", vehicleId);
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

            _logger.LogInformation("Placing bid on vehicle {VehicleId} by bidder {BidderEmail}", vehicleId, bidDto.Bidder.Email);

            try
            {
                var auction = await _auctionRepository.GetByVehicleId(vehicleId);
                if (auction == null)
                {
                    _logger.LogWarning("Attempted to place bid on nonexistent auction for vehicle {VehicleId}", vehicleId);
                    return Result<AuctionDto>.Failure(new Error($"There is no Auction for this vehicle {vehicleId}."));
                }

                var bidderResult = await GetOrCreateBidderAsync(bidDto.Bidder);
                if (bidderResult.IsFailure)
                {
                    _logger.LogWarning("Failed to get or create bidder {BidderEmail}", bidDto.Bidder.Email);
                    return Result<AuctionDto>.Failure(new Error("Failed to find or create bidder."));
                }

                var bid = new Bid(bidderResult.Data!, bidDto.Amount);
                auction.PlaceBid(bid);
                await _auctionRepository.Update(auction);

                _logger.LogInformation("Bid placed successfully on vehicle {VehicleId} for amount {Amount}", vehicleId, bidDto.Amount);

                var auctionDto = _mapper.Map<AuctionDto>(auction);
                return Result<AuctionDto>.Success(auctionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error placing bid on vehicle {VehicleId}", vehicleId);
                return Result<AuctionDto>.Failure(new Error(ex.Message));
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<Result<AuctionDto>> CloseAuctionAsync(string vehicleId)
        {
            var semaphore = GetAuctionLock(vehicleId);
            await semaphore.WaitAsync();

            _logger.LogInformation("Closing auction for vehicle {VehicleId}", vehicleId);

            try
            {
                var auction = await _auctionRepository.GetByVehicleId(vehicleId);
                if (auction == null)
                {
                    _logger.LogWarning("Attempted to close nonexistent auction for vehicle {VehicleId}", vehicleId);
                    return Result<AuctionDto>.Failure(new Error($"There is no Auction for this vehicle {vehicleId}."));
                }

                auction.End();
                await _auctionRepository.Update(auction);

                _logger.LogInformation("Auction closed for vehicle {VehicleId}", vehicleId);

                var auctionDto = _mapper.Map<AuctionDto>(auction);
                return Result<AuctionDto>.Success(auctionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing auction for vehicle {VehicleId}", vehicleId);
                return Result<AuctionDto>.Failure(new Error(ex.Message));
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task<Result<Bidder>> GetOrCreateBidderAsync(BidderDto bidderDto)
        {
            try
            {
                var existingBidderResult = await _bidderService.GetByEmailAsync(bidderDto.Email);
                if (existingBidderResult.IsSuccess)
                {
                    _logger.LogInformation("Existing bidder found for email {Email}", bidderDto.Email);
                    var bidder = _mapper.Map<Bidder>(existingBidderResult.Data);
                    return Result<Bidder>.Success(bidder);
                }

                _logger.LogInformation("Creating new bidder with email {Email}", bidderDto.Email);
                var addBidderResult = await _bidderService.AddBidderAsync(bidderDto);

                if (addBidderResult.IsFailure)
                {
                    _logger.LogWarning("Failed to create bidder for email {Email}", bidderDto.Email);
                    return Result<Bidder>.Failure(new Error("Could not create bidder."));
                }

                var newBidder = _mapper.Map<Bidder>(addBidderResult.Data);
                _logger.LogInformation("Bidder created successfully for email {Email}", bidderDto.Email);
                return Result<Bidder>.Success(newBidder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOrCreateBidderAsync for email {Email}", bidderDto.Email);
                return Result<Bidder>.Failure(new Error("Internal error while retrieving or creating bidder."));
            }
        }

        private static SemaphoreSlim GetAuctionLock(string vehicleId)
        {
            return _auctionLocks.GetOrAdd(vehicleId, _ => new SemaphoreSlim(1, 1));
        }
    }
}
