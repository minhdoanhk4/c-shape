using System;
using System.Threading.Tasks;

namespace CFMS.AuthService.Core.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Token, string Message)> LoginAsync(string username, string password);
        Task<(bool Success, string Message)> RegisterAsync(string username, string password, string roleName, Guid? franchiseId);
    }
}
