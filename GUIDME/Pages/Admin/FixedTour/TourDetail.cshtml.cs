using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUIDME.Pages.Admin.FixedTour
{
    public class TourDetailModel : PageModel
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourImageRepository _tourImageRepository;
        private readonly ITourGuideRepository _tourGuideRepository;

        public Tour Tour { get; set; }
        public List<TourImage> TourImages { get; set; } = new List<TourImage>();
        public List<TourGuide> TourGuideRequests { get; set; } = new List<TourGuide>();

        public TourDetailModel(ITourRepository tourRepository, ITourImageRepository tourImageRepository, ITourGuideRepository tourGuideRepository)
        {
            _tourRepository = tourRepository;
            _tourImageRepository = tourImageRepository;
            _tourGuideRepository = tourGuideRepository;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Tour = await _tourRepository.GetTourById(id);
            if (Tour == null)
            {
                return NotFound();
            }

            TourImages = (await _tourImageRepository.GetTourImagesByTourId(id)).ToList();
            TourGuideRequests = (await _tourGuideRepository.GetAllTourGuides())
                                .Where(r => r.TourId == id)
                                .ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateRequestStatusAsync(int requestId, string status)
        {
            var request = await _tourGuideRepository.GetTourGuideById(requestId);
            if (request == null || request.TourId == null)
            {
                return NotFound(); 
            }

            await _tourGuideRepository.UpdateRequestStatus(requestId, status);

            return RedirectToPage(new { id = request.TourId }); // Sử dụng request.TourId thay vì Tour.TourId
        }
    }
}
