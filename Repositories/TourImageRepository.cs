using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TourImageRepository : ITourImageRepository
    {
        public async Task Add(TourImage tourImage)
        {
            await TourImageDAO.Instance.Add(tourImage);
        }

        public async Task Delete(int id)
        {
            await TourImageDAO.Instance.Delete(id);
        }

        public async Task<IEnumerable<TourImage>> GetAllTourImages()
        {
            return await TourImageDAO.Instance.GetAllTourImages();
        }

        public async Task<TourImage> GetTourImageById(int id)
        {
            return await TourImageDAO.Instance.GetTourImageById(id);
        }

        public async Task<IEnumerable<TourImage>> GetTourImagesByTourId(int tourId)
        {
            return await TourImageDAO.Instance.GetTourImagesByTourId(tourId);
        }

        public async Task Update(TourImage tourImage)
        {
            await TourImageDAO.Instance.Update(tourImage);
        }

        public async Task<int> GetTourImageCount()
        {
            return await TourImageDAO.Instance.GetTourImageCount();
        }
    }
}
