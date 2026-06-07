namespace FlightSearchAPI.Models.Responses
{
    public class FlightResponse
    {
        public FlightResponse(Flight flight, Airport? origin, Airport? destination)
        {
            Id = flight.Id;
            FlightNumber = flight.FlightNumber;
            Airline = flight.Airline;
            Origin = flight.Origin;
            Destination = flight.Destination;
            DepartureDate = flight.DepartureDate;
            DepartureTime = flight.DepartureTime;
            ArrivalTime = flight.ArrivalTime;
            DurationMinutes = flight.DurationMinutes;
            Price = flight.Price;
            Currency = flight.Currency;
            AvailableSeats = flight.AvailableSeats;
            TripType = flight.TripType;
            OriginFullName = origin.Name;
            DestinationFullName = destination.Name;

        }

        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string Origin { get; set; }
        public string OriginFullName { get; set; }
        public string Destination { get; set; }
        public string DestinationFullName { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int AvailableSeats { get; set; }
        public string TripType { get; set; }
    }
}
