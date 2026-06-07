using FlightSearchAPI.Models.Requests;
using FlightSearchAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<SearchController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}


        [HttpGet("origins")]
        public IActionResult GetOrigins()
        {
            try
            {
                var origins = _flightSearchService.GetOrigins();

                if (!origins.Any())
                    return NotFound("No available origins found");

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
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Origin) ||
                string.IsNullOrWhiteSpace(request.Destination))
            {
                return BadRequest("Invalid search request");
            }

            var result = _flightSearchService.SearchFlights(request);

            return Ok(result);
        }

    }
}
