using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrvaWebAplikacija.Model
{
    public class CreateUserDto
    {
        [Required (ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required (ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }
}
