using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAllRatings();
        Task<Rating> GetRatingById(int id);
        Task Add(Rating rating);
        Task Update(Rating rating);
        Task Delete(int id);
        Task<int> GetRatingCount();
    }
}
