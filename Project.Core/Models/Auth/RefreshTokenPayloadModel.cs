using System;

namespace Project.Core.Models.Auth
{
    [Serializable]
    public class RefreshTokenPayloadModel
    {
        public string Claims { get; set; }
        public string UserId { get; set; }
        public byte[] TimeWithKey { get; set; }
    }
}
