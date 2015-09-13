using Newtonsoft.Json;

namespace Parmalen.Contracts.Intent
{
    public class WitOutcome
    {
        [JsonProperty("_text")]
        public string Text { get; set; }

        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("confidence")]
        public decimal Confidence { get; set; }

        [JsonProperty("entities")]
        public WitEntities Entities { get; set; }
    }
}