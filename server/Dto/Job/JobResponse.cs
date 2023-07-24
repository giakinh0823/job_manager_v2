using server.Dto.Log;

namespace server.Dto.Job
{
    public class JobResponse
    {
        public int JobId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Webhook { get; set; }
        public string? Payload { get; set; }
        public string? Header { get; set; }
        public string? Method { get; set; }
        public string? Expression { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual ICollection<LogResponse> Logs { get; set; }
    }
}
