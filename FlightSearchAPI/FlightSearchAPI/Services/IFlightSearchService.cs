using FlightSearchAPI.Models;
using FlightSearchAPI.Models.Requests;
using FlightSearchAPI.Models.Responses;

namespace FlightSearchAPI.Services
{
    public interface IFlightSearchService
    {
        IEnumerable<DestinationResponse> GetDestinations(DestinationRequest request);
        IEnumerable<OriginResponse> GetOrigins();
        IEnumerable<FlightResponse> SearchFlights(FlightSearchRequest request);
        bool CheckIfOriginExists(string origin);
        bool CheckIfDestinationExists(string origin, string destination);
        bool CheckTripType(string tripType);
    }
}