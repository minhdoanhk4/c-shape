using System;
using System.Collections.Generic;

namespace CFMS.AuthService.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        
        // Null means HQ/Admin, Has value means Franchise Manager/Staff
        public Guid? FranchiseId { get; set; }
        
        // Lockout policy properties
        public int FailedLoginAttempts { get; set; } = 0;
        public bool IsLockedOut { get; set; } = false;
        public DateTimeOffset? LockoutEnd { get; set; }
        
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
        
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
