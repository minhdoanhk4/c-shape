using System;
using System.Threading.Tasks;
using CFMS.AuthService.Core.Entities;

namespace CFMS.AuthService.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> ExistsByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task SaveChangesAsync();
    }
}
