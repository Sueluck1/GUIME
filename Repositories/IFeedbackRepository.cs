using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacks();
        Task<Feedback> GetFeedbackById(int id);
        Task Add(Feedback feedback);
        Task Update(Feedback feedback);
        Task Delete(int id);
        Task<int> GetFeedbackCount();
    }
}
