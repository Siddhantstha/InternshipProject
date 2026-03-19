using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class AddUserDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string Role { get; set; } = "Customer";
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
    public class UserStatusDtos
    {
        public int UserId { get; set; }
        public bool isActive {  get; set; }
        public bool isLocked { get; set; }
    }

    public class UserRoleDto
    {
        public int UserId { get; set; }
        public string Role { get; set; } = null!;
    }

    public class DeleteUserDto
    {
        public int UserId { get; set; }
        public int AdminId {  get; set; }
    }
}
