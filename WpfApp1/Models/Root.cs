using Newtonsoft.Json;
using System.Collections.Generic;

namespace Demo.Models
{
    public class Root
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public List<Data> Data { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}