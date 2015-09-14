using Newtonsoft.Json;

namespace Parmalen.Weather
{
    public class Coordinate
    {

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }
}