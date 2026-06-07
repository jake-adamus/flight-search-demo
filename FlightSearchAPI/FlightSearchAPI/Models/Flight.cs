namespace FlightSearchAPI.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
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
