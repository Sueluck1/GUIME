using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITourRepository
    {
        Task<IEnumerable<Tour>> GetAllTours();
        Task<Tour> GetTourById(int id);
        Task Add(Tour tour);
        Task Update(Tour tour);
        Task Delete(int id);
        Task<int> GetTourCount();
        //Fixed
        Task<IEnumerable<Tour>> GetFixedTours();
        Task AddFixedTour(Tour tour);
        Task UpdateFixedTour(Tour tour);
        Task DeleteFixedTour(int id);
    }
}
