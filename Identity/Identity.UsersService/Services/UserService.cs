using System.Threading.Tasks;
using Identity.UsersService.Model;

namespace Identity.UsersService.Services
{
    public class UserService : IUserService
    {
        public Task<User> FindAsync(string username)
        {
            return Task.FromResult(new User
            {
                Username = "demo",
                Password = "demo",      // remember to hash the password
                UserId = 1,
                Email = string.Empty,
                Firstname = string.Empty,
                Lastname = string.Empty,
                Role = string.Empty
            });
        }

        public Task<User> FindAsync(long userId)
        {
            return Task.FromResult(new User
            {
                Username = "demo",
                Password = "demo",      // remember to hash the password
                UserId = 1,
                Email = string.Empty,
                Firstname = string.Empty,
                Lastname = string.Empty,
                Role = string.Empty
            });
        }
    }
}
