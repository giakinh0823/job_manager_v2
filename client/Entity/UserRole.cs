using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace client.Entity
{
    public class UserRole
    {
        [Key]
        [Column("user_id")]
        public int? UserId { get; set; }

        [Key]
        [Column("role_id")]
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual User? User { get; set; }
    }
}
