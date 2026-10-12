using System;
using System.Collections.Generic;
using System.Text;

namespace PrvaWebAplikacija.Model
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; } = null!;
    }
}
