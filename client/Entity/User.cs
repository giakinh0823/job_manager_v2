using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace client.Entity
{
    public partial class User
    {
        public User()
        {
            Logs = new HashSet<Log>();
            Jobs = new HashSet<Job>();
            UserRoles = new HashSet<UserRole>();
        }

        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("password")]
        public string? Password { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
