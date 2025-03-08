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
            // Fetch the tour by its ID
            Tour = await _tourRepository.GetTourById(id);
            if (Tour == null)
            {
                return NotFound(); // Return 404 if tour not found
            }

            // Fetch images associated with the tour
            TourImages = (await _tourImageRepository.GetTourImagesByTourId(id)).ToList();

            // Fetch guide requests associated with the tour
            TourGuideRequests = (await _tourGuideRepository.GetAllTourGuides())
                                .Where(r => r.TourId == id)
                                .ToList();
            return Page(); // Return the page with the tour details
        }

        // Method to update the status of a tour guide's request
        public async Task<IActionResult> OnPostUpdateRequestStatusAsync(int requestId, string status)
        {
            // Fetch the tour guide request by ID
            var request = await _tourGuideRepository.GetTourGuideById(requestId);
            if (request == null || request.TourId == null)
            {
                return NotFound(); // Return 404 if request not found
            }

            // Update the status of the guide's request
            await _tourGuideRepository.UpdateRequestStatus(requestId, status);

            // Redirect to the same tour detail page after updating the status
            return RedirectToPage("/Admin/FixedTour/TourDetail", new { id = request.TourId });
        }
    }
}
