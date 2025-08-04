using Auction.Management.Application.Dto;
using Auction.Management.Application.Interfaces;
using Auction.Management.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleDto vehicleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vehicleService.AddVehicle(vehicleDto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(AddVehicle), new { id = vehicleDto.Id }, result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicles(
                                                    [FromQuery] VehicleType? type = null,
                                                    [FromQuery] string? manufacturer = null,
                                                    [FromQuery] string? model = null,
                                                    [FromQuery] int? year = null)
        {
            var result = await _vehicleService.SearchAsync(type, manufacturer, model, year);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

    }
}
