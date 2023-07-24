using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public partial class Job
    {
        public Job()
        {
            Logs = new HashSet<Log>();
        }

        [Key]
        [Column("job_id")]
        public int JobId { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("webhook")]
        public string? Webhook { get; set; }
        [Column("payload")]
        public string? Payload { get; set; }
        [Column("header")]
        public string? Header { get; set; }
        [Column("method")]
        public string? Method { get; set; }
        [Column("expression")]
        public string? Expression { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("status")]
        public string? Status { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
