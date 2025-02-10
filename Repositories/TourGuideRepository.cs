using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TourGuideRepository : ITourGuideRepository
    {
        public async Task Add(TourGuide tourGuide)
        {
            await TourGuideDAO.Instance.Add(tourGuide);
        }

        public async Task Delete(int id)
        {
            await TourGuideDAO.Instance.Delete(id);
        }

        public async Task<IEnumerable<TourGuide>> GetAllTourGuides()
        {
            return await TourGuideDAO.Instance.GetAllTourGuides();
        }

        public async Task<TourGuide> GetTourGuideById(int id)
        {
            return await TourGuideDAO.Instance.GetTourGuideById(id);
        }

        public async Task Update(TourGuide tourGuide)
        {
            await TourGuideDAO.Instance.Update(tourGuide);
        }

        public async Task<int> GetTourGuideCount()
        {
            return await TourGuideDAO.Instance.GetTourGuideCount();
        }
        public async Task AddRequest(int tourId, int guideId)
        {
            var newRequest = new TourGuide
            {
                TourId = tourId,
                GuideId = guideId,
                Status = "Pending",
                RequestDate = DateTime.Now
            };

            await TourGuideDAO.Instance.Add(newRequest);
        }
        public async Task UpdateRequestStatus(int requestId, string status)
        {
            var request = await TourGuideDAO.Instance.GetTourGuideById(requestId);
            if (request != null)
            {
                request.Status = status;
                await TourGuideDAO.Instance.Update(request);
            }
        }

    }
}
