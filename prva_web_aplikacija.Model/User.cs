using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prva_web_aplikacija.Model
{   [Table("users")]
    public partial class User
    {
        [Key]
        [Column("user_id")]
        public Guid User_id { get; set; }

        [Column("username")]
        public string Username { get; set; } = null!;

        [Column("password_hash")]
        public string Password_hash { get; set; } = null!;

        [Column("role")]
        public string Role { get; set; } = null!;
    }
}
