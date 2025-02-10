using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        public async Task Add(Feedback feedback)
        {
            await FeedbackDAO.Instance.Add(feedback);
        }

        public async Task Delete(int id)
        {
            await FeedbackDAO.Instance.Delete(id);
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacks()
        {
            return await FeedbackDAO.Instance.GetAllFeedbacks();
        }

        public async Task<Feedback> GetFeedbackById(int id)
        {
            return await FeedbackDAO.Instance.GetFeedbackById(id);
        }

        public async Task Update(Feedback feedback)
        {
            await FeedbackDAO.Instance.Update(feedback);
        }

        public async Task<int> GetFeedbackCount()
        {
            return await FeedbackDAO.Instance.GetFeedbackCount();
        }
    }
}
