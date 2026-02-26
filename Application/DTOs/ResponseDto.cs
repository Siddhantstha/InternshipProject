using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    
    public class LoginResponseDto
    {
        public string message { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
