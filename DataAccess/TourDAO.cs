using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TourDAO : SingletonBase<TourDAO>
    {
        public async Task<IEnumerable<Tour>> GetAllTours()
        {
            return await _context.Tours.Include(t => t.Category) // Load dữ liệu Category
            .ToListAsync();
        }

        public async Task<Tour> GetTourById(int id)
        {
            return await _context.Tours.FirstOrDefaultAsync(t => t.TourId == id);
        }

        public async Task Add(Tour tour)
        {
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Tour tour)
        {
            var existingTour = await GetTourById(tour.TourId);
            if (existingTour != null)
            {
                _context.Entry(existingTour).CurrentValues.SetValues(tour);
            }
            else
            {
                _context.Tours.Add(tour);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var tour = await GetTourById(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTourCount()
        {
            return await _context.Tours.CountAsync();
        }
    }
}
