using Newtonsoft.Json;

namespace Demo.Models
{
    public class Data
    {
        [JsonProperty("c_id")]
        public string CId { get; set; }
        [JsonProperty("detailedstyle")]
        public string DetailedStyle { get; set; }
        [JsonProperty("fee_type")]
        public string FeeType { get; set; }
        [JsonProperty("file_size")]
        public string FileSize { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("money")]
        public string Money { get; set; }
        [JsonProperty("mx_id")]
        public string MxId { get; set; }
        [JsonProperty("mx_type")]
        public string MxType { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("rendering")]
        public string Rendering { get; set; }
        [JsonProperty("thumb")]
        public string Thumb { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}