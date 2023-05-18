using Newtonsoft.Json;

namespace Project.Core.Models.Auth
{
    public class DtoRefreshToken
    {
        [JsonProperty(PropertyName = "refreshToken")]
        public string RefreshToken { get; set; }
    }
}
