using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prva_web_aplikacija.Model
{
    public class CreateUserDto
    {
        [Required (ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required (ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }
    public class UserDto
    {
        public Guid User_id { get; set; }
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
    }

   
}
