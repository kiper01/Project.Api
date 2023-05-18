using Newtonsoft.Json;
using Project.Core.Models.Dto;

namespace Project.Core.Models.Auth
{
    public class DtoTokenResponse
    {
        [JsonProperty(PropertyName = "accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expiresIn")]
        public int? ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "user")]
        public DtoUser User { get; set; }
    }
}
