using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Entity
{
    public partial class PaymentInfo
    {
        [Key]
        [Column("payment_id")]
        public int PaymentId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }
        [Column("payment_amount")]
        public decimal PaymentAmount { get; set; }
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        [Column("end_date")]
        public DateTime EndDate { get; set; }
        [Column("status")]
        public int Status { get; set; }
    }
}
