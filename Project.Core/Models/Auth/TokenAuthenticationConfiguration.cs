namespace Project.Core.Models.Auth
{
    public class TokenAuthenticationConfiguration
    {
        public string Path { get; set; } = "/token";

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public string ApiKey { get; set; }
        public int ExpirationSecondsAmount { get; set; }

    }
}
