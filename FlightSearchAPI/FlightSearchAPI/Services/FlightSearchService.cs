using FlightSearchAPI.Data;
using FlightSearchAPI.Models;
using FlightSearchAPI.Models.Requests;
using FlightSearchAPI.Models.Responses;
using System.Diagnostics.Eventing.Reader;

namespace FlightSearchAPI.Services
{

    public class FlightSearchService : IFlightSearchService
    {
        private readonly FlightDataContext _context;
        private readonly ILogger<FlightSearchService> _logger;
        private readonly List<string> _tripTypes = new List<string>() { "oneway", "return" };

        public FlightSearchService(FlightDataContext context, ILogger<FlightSearchService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IEnumerable<OriginResponse> GetOrigins()
        {
            _logger.LogInformation("Fetching available origins from flights");

            return _context.Flights
                .Select(f =>f.Origin)
                .Distinct()
                .OrderBy(o => o)
                .Select(origin=>
                {
                    var airport = _context.Airports.First(airport => airport.Code == origin);
                    return new OriginResponse(airport.Code, airport.Name);
                })
                .ToList();
        }


        public IEnumerable<DestinationResponse> GetDestinations(DestinationRequest request)
        {
            _logger.LogInformation("Fetching destinations for {Origin}", request.Origin);

            if (string.IsNullOrWhiteSpace(request.Origin))
                throw new ArgumentException("Origin cannot be empty");

            // Check if airport exists
            if (!_context.Airports.Any(a =>
                a.Code.Equals(request.Origin, StringComparison.CurrentCultureIgnoreCase)))
            {
                return Enumerable.Empty<DestinationResponse>();
            }

            var destinations = _context.Routes
                .Where(r => r.Origin.Equals(request.Origin, StringComparison.CurrentCultureIgnoreCase))
                .SelectMany(r => r.Destinations)
                .Distinct()
                .Select(destination =>
                {
                    var airport = _context.Airports.First(airport => airport.Code == destination);
                    return new DestinationResponse(destination, airport.Name);
                })
                .ToList();

            return destinations;
        }

        public IEnumerable<FlightResponse> SearchFlights(FlightSearchRequest request)
        {
            _logger.LogInformation(
                "Searching flights from {Origin} to {Destination}",
                request.Origin, request.Destination);

            var query = _context.Flights.AsQueryable();

            if (request.TripType?.Equals("return", StringComparison.OrdinalIgnoreCase) == true)
            {
                // Reverse origin/destination for return flights
                query = query.Where(f =>
                    f.Origin.Equals(request.Destination, StringComparison.OrdinalIgnoreCase) &&
                    f.Destination.Equals(request.Origin, StringComparison.OrdinalIgnoreCase)
                );
            }
            else
            {
                query = query.Where(f =>
                    f.Origin.Equals(request.Origin, StringComparison.OrdinalIgnoreCase) &&
                    f.Destination.Equals(request.Destination, StringComparison.OrdinalIgnoreCase)
                );
            }

            if (request.DepartureDate.HasValue)
            {
                query = query.Where(f => f.DepartureDate.Date == request.DepartureDate.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(request.TripType))
            {
                query = query.Where(f =>
                    f.TripType.Equals(request.TripType, StringComparison.CurrentCultureIgnoreCase));
            }

            var flights = query
                .Select(flight => AddFullAirportNames(flight))
                .ToList();

            return flights;
            
        }

        public bool CheckIfOriginExists(string origin)
        {
            return _context.Flights
                .Select(f => f.Origin)
                .Distinct()
                .Any(originInDb => originInDb.Equals(origin, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool CheckIfDestinationExists(string origin, string destination)
        {
            return _context.Routes
                .Where(r => r.Origin.Equals(origin, StringComparison.CurrentCultureIgnoreCase))
                .SelectMany(r => r.Destinations)
                .Distinct()
                .Any(destinationInDb => destination.Equals(destinationInDb, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool CheckTripType(string tripType)
        {
            return _tripTypes.Any(type => tripType.Equals(type, StringComparison.CurrentCultureIgnoreCase));
        }


        private FlightResponse AddFullAirportNames(Flight flight)
        {
            var origin = _context.Airports.FirstOrDefault(airport => airport.Code == flight.Origin);
            var destination = _context.Airports.FirstOrDefault(airport => airport.Code == flight.Origin);
            return new FlightResponse(flight, origin, destination);
        }
    }
}

