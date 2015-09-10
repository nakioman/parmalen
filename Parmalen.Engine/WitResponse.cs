using System.Collections.Generic;
using Newtonsoft.Json;

namespace Parmalen.Engine
{
    public class WitResponse
    {
        [JsonProperty("msg_id")]
        public string MessageId { get; set; }

        [JsonProperty("_text")]
        public string Text { get; set; }

        [JsonProperty("outcomes")]
        public IEnumerable<WitOutcome> Outcomes { get; set; }
    }
}