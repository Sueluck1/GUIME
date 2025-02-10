using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITourImageRepository
    {
        Task<IEnumerable<TourImage>> GetAllTourImages();
        Task<TourImage> GetTourImageById(int id);
        Task<IEnumerable<TourImage>> GetTourImagesByTourId(int tourId);
        Task Add(TourImage tourImage);
        Task Update(TourImage tourImage);
        Task Delete(int id);
        Task<int> GetTourImageCount();
    }
}
