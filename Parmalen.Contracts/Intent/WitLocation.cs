using Newtonsoft.Json;

namespace Parmalen.Contracts.Intent
{
    public class WitLocation
    {
        [JsonProperty("suggested")]
        public bool Suggested { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}