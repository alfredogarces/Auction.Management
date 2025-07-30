using Auction.Management.Application.Dto;
using Auction.Management.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpPost("{vehicleId}/start")]
        public async Task<IActionResult> StartAuction(string vehicleId)
        {
            var result = await _auctionService.StartAuctionAsync(vehicleId);
            if (result.IsFailure)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpPost("{vehicleId}/close")]
        public async Task<IActionResult> CloseAuction(string vehicleId)
        {
            var result = await _auctionService.CloseAuctionAsync(vehicleId);
            if (result.IsFailure)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpPost("{vehicleId}/bid")]
        public async Task<IActionResult> PlaceBid(string vehicleId, [FromBody] BidDto bidDto)
        {
            var result = await _auctionService.PlaceBidAsync(vehicleId, bidDto);
            if (result.IsFailure)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }
    }
}
