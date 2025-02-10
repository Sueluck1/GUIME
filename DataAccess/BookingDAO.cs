using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BookingDAO : SingletonBase<BookingDAO>
    {
        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            return await _context.Bookings
                .Include(b => b.User)  // Load thông tin User
                .Include(b => b.Tour)  // Load thông tin Tour
                .ToListAsync();
        }


        public async Task<Booking> GetBookingById(int id)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task Add(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Booking booking)
        {
            var existingBooking = await GetBookingById(booking.BookingId);
            if (existingBooking != null)
            {
                _context.Entry(existingBooking).CurrentValues.SetValues(booking);
            }
            else
            {
                _context.Bookings.Add(booking);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var booking = await GetBookingById(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetBookingCount()
        {
            return await _context.Bookings.CountAsync();
        }
        public async Task<List<Booking>> GetBookingsByUserId(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Tour) 
                .Where(b => b.UserId == userId) 
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }
    }
}
