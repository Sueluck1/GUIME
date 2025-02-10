using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BookingRepository : IBookingRepository
    {
        public async Task Add(Booking booking)
        {
            await BookingDAO.Instance.Add(booking);
        }

        public async Task Delete(int id)
        {
            await BookingDAO.Instance.Delete(id);
        }

        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            return await BookingDAO.Instance.GetAllBookings();
        }

        public async Task<Booking> GetBookingById(int id)
        {
            return await BookingDAO.Instance.GetBookingById(id);
        }

        public async Task Update(Booking booking)
        {
            await BookingDAO.Instance.Update(booking);
        }
        public async Task<List<Booking>> GetBookingsByUserId(int userId)
        {
            return await BookingDAO.Instance.GetBookingsByUserId(userId);
        }
        public async Task<int> GetBookingCount()
        {
            return await BookingDAO.Instance.GetBookingCount();
        }
    }
}
