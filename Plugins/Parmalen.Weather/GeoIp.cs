using Newtonsoft.Json;

namespace Parmalen.Weather
{
    public class GeoIp
    {

        [JsonProperty("dma_code")]
        public string DmaCode { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("asn")]
        public string Asn { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("offset")]
        public string Offset { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("isp")]
        public string Isp { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("area_code")]
        public string AreaCode { get; set; }

        [JsonProperty("continent_code")]
        public string ContinentCode { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country_code3")]
        public string CountryCode3 { get; set; }
    }
}