using System;
using System.Collections.Generic;

namespace CFMS.AuthService.Core.Entities
{
    public class Permission
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty; // e.g., "Inventory", "Order"
        public string Action { get; set; } = string.Empty; // e.g., "Read", "Write", "Delete"
        
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
        
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}
