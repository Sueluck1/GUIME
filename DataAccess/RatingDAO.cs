using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RatingDAO : SingletonBase<RatingDAO>
    {
        public async Task<IEnumerable<Rating>> GetAllRatings()
        {
            return await _context.Ratings.ToListAsync();
        }

        public async Task<Rating> GetRatingById(int id)
        {
            return await _context.Ratings.FirstOrDefaultAsync(r => r.RatingId == id);
        }

        public async Task Add(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Rating rating)
        {
            var existingRating = await GetRatingById(rating.RatingId);
            if (existingRating != null)
            {
                _context.Entry(existingRating).CurrentValues.SetValues(rating);
            }
            else
            {
                _context.Ratings.Add(rating);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var rating = await GetRatingById(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetRatingCount()
        {
            return await _context.Ratings.CountAsync();
        }
    }
}
