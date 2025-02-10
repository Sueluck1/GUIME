using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllBookings();
        Task<Booking> GetBookingById(int id);
        Task Add(Booking booking);
        Task Update(Booking booking);
        Task Delete(int id);
        Task<List<Booking>> GetBookingsByUserId(int userId);
        Task<int> GetBookingCount();
    }
}
