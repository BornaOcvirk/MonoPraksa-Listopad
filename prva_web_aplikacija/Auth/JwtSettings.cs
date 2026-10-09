namespace prva_web_aplikacija.Auth
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Key { get; set; } = null!;
        public int ExpiresInMinutes { get; set; } = 60;
    }
}
