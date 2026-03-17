using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class UserDTOs
    {
        public class ViewUserDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Role { get; set; } = null!;
            public string Phone { get; set; } = null!;
            public string Address { get; set; } = null!;
            public bool IsDeleted { get; set; } = false;
            public string Status =>
                 IsLocked ? "Locked" :
                 IsActive ? "Active" : "Deactivated";
            [JsonIgnore]
            public bool IsActive { get; set; } = true;
            [JsonIgnore]
            public bool IsLocked { get; set; } = false;
        }
        public class UpdateUserDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
            public string ConfirmPassword { get; set; } = null!;
            public string Phone { get; set; } = null!;
            public string Address { get; set; } = null!;
            public bool IsActive { get; set; } = true;
           
        }
        public class LoginUserDto
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }
        
    }
}
