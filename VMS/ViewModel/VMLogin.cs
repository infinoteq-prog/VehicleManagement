using System;
using System.Collections.Generic;
using VMS.Models;

namespace VMS.ViewModel
{
    public class VMLogin
    {
        public Int32 Id { get; set; }

        public string? UserName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? LastLoginTime { get; set; }

        public string? Message { get; set; }

        public bool Success { get; set; }

        public bool IsActive { get; set; }

        public int? RoleId { get; set; }

        public string? RoleName { get; set; }

        public string? UserId { get; set; }

        public string? TransporterName { get; set; }
    }
}
