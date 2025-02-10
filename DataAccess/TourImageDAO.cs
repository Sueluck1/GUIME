using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TourImageDAO : SingletonBase<TourImageDAO>
    {
        public async Task<IEnumerable<TourImage>> GetAllTourImages()
        {
            return await _context.TourImages.ToListAsync();
        }

        public async Task<TourImage> GetTourImageById(int id)
        {
            return await _context.TourImages.FirstOrDefaultAsync(ti => ti.ImageId == id);
        }

        public async Task<IEnumerable<TourImage>> GetTourImagesByTourId(int tourId)
        {
            return await _context.TourImages.Where(ti => ti.TourId == tourId).ToListAsync();
        }

        public async Task Add(TourImage tourImage)
        {
            _context.TourImages.Add(tourImage);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TourImage tourImage)
        {
            var existingImage = await GetTourImageById(tourImage.ImageId);
            if (existingImage != null)
            {
                _context.Entry(existingImage).CurrentValues.SetValues(tourImage);
            }
            else
            {
                _context.TourImages.Add(tourImage);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var tourImage = await GetTourImageById(id);
            if (tourImage != null)
            {
                _context.TourImages.Remove(tourImage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTourImageCount()
        {
            return await _context.TourImages.CountAsync();
        }
    }
}
