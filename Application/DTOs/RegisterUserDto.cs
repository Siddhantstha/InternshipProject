using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class RegisterUserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!; 
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
