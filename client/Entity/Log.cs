﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace client.Entity
{
    public partial class Log
    {
        [Key]
        [Column("log_id")]
        public int LogId { get; set; }
        [Column("job_id")]
        public int? JobId { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("start_time")]
        public DateTime? StartTime { get; set; }
        [Column("end_time")]
        public DateTime? EndTime { get; set; }
        [Column("status")]
        public string? Status { get; set; }
        [Column("output")]
        public string? Output { get; set; }
        public virtual Job? Job { get; set; }
        public virtual User? User { get; set; }
    }
}
