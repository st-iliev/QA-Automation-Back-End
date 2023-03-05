using Newtonsoft.Json;

namespace RestSharpAPITests
{
    public class ContactObj
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
    }
}