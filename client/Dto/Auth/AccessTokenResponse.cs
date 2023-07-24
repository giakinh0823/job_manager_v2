namespace client.Dto.Auth
{
    public class AccessTokenResponse
    {
        public DateTime ExpiresTime { get; set; }
        public List<string> Roles { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
