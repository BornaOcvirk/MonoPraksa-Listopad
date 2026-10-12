using System;
using System.Collections.Generic;
using System.Text;

namespace PrvaWebAplikacija.Model
{
    public class UserDto
    {
        public Guid User_id { get; set; }
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
