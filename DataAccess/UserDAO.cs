using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDAO : SingletonBase<UserDAO>
    {
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            var existingUser = await GetUserById(user.UserId);
            if (existingUser != null)
            {
                _context.Entry(existingUser).CurrentValues.SetValues(user);
            }
            else
            {
                _context.Users.Add(user);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await GetUserById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersByRole(string role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .ToListAsync();
        }


        public async Task<int> GetUserCount()
        {
            return await _context.Users.CountAsync();
        }
    }
}
