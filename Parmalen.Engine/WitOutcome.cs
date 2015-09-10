using Newtonsoft.Json;

namespace Parmalen.Engine
{
    public class WitOutcome
    {
        [JsonProperty("_text")]
        public string Text { get; set; }

        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("confidence")]
        public decimal Confidence { get; set; }
    }
}