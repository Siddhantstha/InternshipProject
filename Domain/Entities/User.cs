using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User
    {
        public int Id {  get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "Customer";   
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool isActive { get; set; } = true;
        public bool isLocked { get; set; } = false;
        public bool isDeleted { get; set; } = false;
    }
}

