using MultiTenant.Core.DTOs;

namespace MultiTenant.Core.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(
            string email,
            string password);

        Task<AuthenticationResult> LoginAsync(
            string email,
            string password);

        Task<AuthenticationResult> RefreshTokenAsync(
            string token,
            string refreshToken);
    }
}
