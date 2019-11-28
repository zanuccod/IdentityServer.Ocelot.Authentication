using System.Threading.Tasks;
using Identity.UsersService.Model;

namespace Identity.UsersService.Services
{
    public interface IUserService
    {
        Task<User> FindAsync(string username);
        Task<User> FindAsync(long userId);
    }
}
