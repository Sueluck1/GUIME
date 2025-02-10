using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITourGuideRepository
    {
        Task<IEnumerable<TourGuide>> GetAllTourGuides();
        Task<TourGuide> GetTourGuideById(int id);
        Task Add(TourGuide tourGuide);
        Task Update(TourGuide tourGuide);
        Task Delete(int id);
        Task<int> GetTourGuideCount();
        // 🔹 Thêm yêu cầu hướng dẫn viên dẫn tour
        Task AddRequest(int tourId, int guideId);

        // 🔹 Cập nhật trạng thái yêu cầu
        Task UpdateRequestStatus(int requestId, string status);

    }
}
