using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task Add(User user)
        {
            await UserDAO.Instance.Add(user);
        }

        public async Task Delete(int id)
        {
            await UserDAO.Instance.Delete(id);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await UserDAO.Instance.GetAllUsers();
        }

        public async Task<User> GetUserById(int id)
        {
            return await UserDAO.Instance.GetUserById(id);
        }

        public async Task Update(User user)
        {
            await UserDAO.Instance.Update(user);
        }
        public async Task<User> Login(string username, string password)
        {
            return await UserDAO.Instance.Login(username, password);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await UserDAO.Instance.GetUserByEmail(email);
        }

        public async Task<IEnumerable<User>> GetUsersWithUserRole()
        {
            return await UserDAO.Instance.GetUsersByRole("User");
        }
        public async Task<int> GetUserCount()
        {
            return await UserDAO.Instance.GetUserCount();
        }
    }

}
