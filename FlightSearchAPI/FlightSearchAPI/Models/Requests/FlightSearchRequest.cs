namespace FlightSearchAPI.Models.Requests
{
    public class FlightSearchRequest
    {
        public string Origin { get; set; } = default!;
        public string Destination { get; set; } = default!;
        public DateTime? DepartureDate { get; set; }
        public string? TripType { get; set; } // "OneWay" / "Return"
    }

}
