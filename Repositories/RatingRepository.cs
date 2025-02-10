using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RatingRepository : IRatingRepository
    {
        public async Task Add(Rating rating)
        {
            await RatingDAO.Instance.Add(rating);
        }

        public async Task Delete(int id)
        {
            await RatingDAO.Instance.Delete(id);
        }

        public async Task<IEnumerable<Rating>> GetAllRatings()
        {
            return await RatingDAO.Instance.GetAllRatings();
        }

        public async Task<Rating> GetRatingById(int id)
        {
            return await RatingDAO.Instance.GetRatingById(id);
        }

        public async Task Update(Rating rating)
        {
            await RatingDAO.Instance.Update(rating);
        }

        public async Task<int> GetRatingCount()
        {
            return await RatingDAO.Instance.GetRatingCount();
        }
    }
}
