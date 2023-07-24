using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace client.Dto.LogRes
{
    public class LogResponse
    {
        public int LogId { get; set; }
        public int? JobId { get; set; }
        public int? UserId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Status { get; set; }
        public string? Output { get; set; }
    }
}
