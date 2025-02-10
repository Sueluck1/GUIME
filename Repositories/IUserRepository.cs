using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task Add(User user);
        Task Update(User user);
        Task Delete(int id);
        Task<User> Login(string username, string password);
        Task<User> GetUserByEmail(string email);

        Task<IEnumerable<User>> GetUsersWithUserRole();
        Task<int> GetUserCount();
    }
}
