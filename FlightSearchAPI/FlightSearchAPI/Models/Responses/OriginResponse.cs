namespace FlightSearchAPI.Models.Responses
{
    public class OriginResponse
    {
        public OriginResponse(string code, string name)
        {
            Code = code;
            Name = name;
        }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
