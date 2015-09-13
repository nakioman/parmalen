using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Parmalen.Contracts.Intent
{
    public class WitEntities
    {
        [JsonProperty("location")]
        public IEnumerable<WitLocation> Locations { get; set; }

        [JsonProperty("datetime")]
        public IEnumerable<WitDateTime> DateTimes { get; set; }
    }
}