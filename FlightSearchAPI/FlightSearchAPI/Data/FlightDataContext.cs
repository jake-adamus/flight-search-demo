using FlightSearchAPI.Models;
using System.Text.Json;
using Route = FlightSearchAPI.Models.Route;

namespace FlightSearchAPI.Data
{
    public class FlightDataContext
    {
        public List<Airport> Airports { get; set; }
        public List<Flight> Flights { get; set; }
        public List<Route> Routes { get; set; }

        public FlightDataContext()
        {
            Airports = JsonSerializer.Deserialize<List<Airport>>(File.ReadAllText(
                "./bin/Debug/net10.0/airports.json"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
            Flights = JsonSerializer.Deserialize<List<Flight>>(File.ReadAllText(
                "./bin/Debug/net10.0/flights.json"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            Routes = JsonSerializer.Deserialize<List<Route>>(File.ReadAllText(
                "./bin/Debug/net10.0/routes.json"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}
