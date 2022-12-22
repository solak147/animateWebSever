using AnimateLibrary;
using System.Threading.Tasks;

namespace Animate.Data.Service
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginModel loginModel);

        Task LogoutAsync();
    }
}