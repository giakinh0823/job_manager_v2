﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Entity
{
    public class Role
    {
        [Key]
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("name")]
        public string? Name { get; set; }
    }
}
