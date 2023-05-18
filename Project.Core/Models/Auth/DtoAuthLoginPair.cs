using Newtonsoft.Json;

namespace Project.Core.Models.Auth
{
    public class DtoAuthLoginPair
    {
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
