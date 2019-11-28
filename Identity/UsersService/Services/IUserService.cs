using System.Threading.Tasks;
using UsersService.Model;

namespace UsersService.Services
{
    public interface IUserService
    {
        Task<User> FindAsync(string username);
        Task<User> FindAsync(long userId);
    }
}
