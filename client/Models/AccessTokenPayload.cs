namespace client.Models
{
    public class AccessTokenPayload
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
