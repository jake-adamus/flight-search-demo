using FlightSearchAPI.Models.Requests;
using FlightSearchAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace FlightSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private IFlightSearchService _flightSearchService;
        private ILogger<FlightController> _logger;

        public FlightController(IFlightSearchService flightSearchService, ILogger<FlightController> logger)
        {
            _flightSearchService = flightSearchService;
            _logger = logger;
        }


        [HttpGet("origins")]
        public IActionResult GetOrigins()
        {
            try
            {
                var origins = _flightSearchService.GetOrigins();

                if (!origins.Any())
                    return NotFound("No available origins found");

                _logger.LogInformation("Origins requested");
                return Ok(origins);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching origins");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("destinations")]
        public IActionResult GetDestinations(DestinationRequest request)
        {
            try
            {
                var result = _flightSearchService.GetDestinations(request);

                if (!result.Any())
                    return NotFound($"No destinations found for '{request.Origin}'");
                
                _logger.LogInformation("Destinations requested.");
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error fetching destinations");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("search")]
        public IActionResult SearchFlights([FromBody] FlightSearchRequest request)
        {
            try
            {
                if (request == null ||
                string.IsNullOrWhiteSpace(request.Origin) ||
                string.IsNullOrWhiteSpace(request.Destination))
                {
                    _logger.LogError("Invalid search request.");
                    return BadRequest("Invalid search request");
                }

                if (!_flightSearchService.CheckIfOriginExists(request.Origin))
                {
                    _logger.LogInformation("No data for selected origin.");
                    return NotFound("No data for selected origin.");
                }

                if (!_flightSearchService.CheckIfDestinationExists(request.Origin, request.Destination))
                {
                    _logger.LogInformation("No data for selected destination.");
                    return NotFound("No data for selected destination.");
                }

                if (!_flightSearchService.CheckTripType(request.TripType))
                {
                    _logger.LogInformation("No data for selected trip type.");
                    return NotFound("No data for selected trip type.");
                }

                var result = _flightSearchService.SearchFlights(request);
                _logger.LogInformation("Flights fetched.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching flights");
                return BadRequest(ex.Message);
            }
            
        }
    }
}
