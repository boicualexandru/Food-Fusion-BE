﻿using System.ComponentModel.DataAnnotations;

namespace Services.Authentication
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string FullName { get; set; }
    }
}
