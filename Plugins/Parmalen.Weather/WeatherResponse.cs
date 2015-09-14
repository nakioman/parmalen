using System.Collections.Generic;
using Newtonsoft.Json;

namespace Parmalen.Weather
{
    public class WeatherResponse
    {

        [JsonProperty("city")]
        public City City { get; set; }

        [JsonProperty("cod")]
        public string Cod { get; set; }

        [JsonProperty("message")]
        public double Message { get; set; }

        [JsonProperty("cnt")]
        public int Cnt { get; set; }

        [JsonProperty("list")]
        public IList<Forecast> Forecasts { get; set; }
    }
}