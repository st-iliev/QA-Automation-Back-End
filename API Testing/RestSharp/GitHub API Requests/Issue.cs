using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GitHub_API_Requests
{
    public class Issue
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }
        [JsonPropertyName("number")]
        public long? Number { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
       
    }
}
