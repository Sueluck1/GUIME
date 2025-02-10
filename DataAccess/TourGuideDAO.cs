using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TourGuideDAO : SingletonBase<TourGuideDAO>
    {
        public async Task<IEnumerable<TourGuide>> GetAllTourGuides()
        {
            return await _context.TourGuides
                .Include(tg => tg.Guide) // Load dữ liệu hướng dẫn viên (User)
                .Include(tg => tg.Tour)  // Load dữ liệu Tour
                .ToListAsync();
        }

        public async Task<TourGuide> GetTourGuideById(int id)
        {
            return await _context.TourGuides.FirstOrDefaultAsync(tg => tg.TourGuideId == id);
        }

        public async Task Add(TourGuide tourGuide)
        {
            _context.TourGuides.Add(tourGuide);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TourGuide tourGuide)
        {
            var existingTourGuide = await GetTourGuideById(tourGuide.TourGuideId);
            if (existingTourGuide != null)
            {
                _context.Entry(existingTourGuide).CurrentValues.SetValues(tourGuide);
            }
            else
            {
                _context.TourGuides.Add(tourGuide);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var tourGuide = await GetTourGuideById(id);
            if (tourGuide != null)
            {
                _context.TourGuides.Remove(tourGuide);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTourGuideCount()
        {
            return await _context.TourGuides.CountAsync();
        }
    }
}
