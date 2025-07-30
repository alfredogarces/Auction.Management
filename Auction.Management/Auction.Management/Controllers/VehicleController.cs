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

        [HttpPost("suv")]
        public async Task<IActionResult> AddSUV([FromBody] SuvDto suvDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vehicleService.AddVehicle(suvDto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(AddSUV), new { id = suvDto.Id }, result.Data);
        }

        [HttpPost("sedan")]
        public async Task<IActionResult> AddSedan([FromBody] SedanDto sedanDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vehicleService.AddVehicle(sedanDto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(AddSedan), new { id = sedanDto.Id }, result.Data);
        }

        [HttpPost("truck")]
        public async Task<IActionResult> AddTruck([FromBody] TruckDto truckDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vehicleService.AddVehicle(truckDto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(AddTruck), new { id = truckDto.Id }, result.Data);
        }

        [HttpPost("hatchback")]
        public async Task<IActionResult> AddHatchback([FromBody] HatchbackDto hatchbackDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vehicleService.AddVehicle(hatchbackDto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(AddHatchback), new { id = hatchbackDto.Id }, result.Data);
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
