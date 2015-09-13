using System;
using Newtonsoft.Json;

namespace Parmalen.Contracts.Intent
{
    public class WitDateTime
    {
        [JsonProperty("grain")]
        public string Grain { get; set; }

        [JsonProperty("value")]
        public DateTimeOffset Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}