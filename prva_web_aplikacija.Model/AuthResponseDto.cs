using System;
using System.Collections.Generic;
using System.Text;

namespace prva_web_aplikacija.Model
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; } = null!;
    }
}
