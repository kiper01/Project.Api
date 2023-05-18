using Newtonsoft.Json;

namespace Project.Core.Models.Auth
{
    public class DtoAuthApiKey
    {
        [JsonProperty(PropertyName = "apiKey")]
        public string ApiKey { get; set; }
    }
}
