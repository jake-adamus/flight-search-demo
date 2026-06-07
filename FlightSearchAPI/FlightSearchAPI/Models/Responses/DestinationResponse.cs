namespace FlightSearchAPI.Models.Responses
{
    public class DestinationResponse
    {
        public DestinationResponse(string code, string name)
        {
            Code = code;
            Name = name;
        }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
