namespace client.Dto.Auth
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; }
        public List<string> Roles { get; set; }
        public DateTime ExpiresTime { get; set; }
    }
}
