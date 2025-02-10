using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Security.Claims;

namespace GUIDME.Pages.Guide.FixedTour
{
    public class RequestForTourModel : PageModel
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourImageRepository _tourImageRepository;
        private readonly ITourGuideRepository _tourGuideRepository;

        public Tour Tour { get; set; }
        public List<TourImage> TourImages { get; set; } = new List<TourImage>();

        public RequestForTourModel(
            ITourRepository tourRepository,
            ITourImageRepository tourImageRepository,
            ITourGuideRepository tourGuideRepository)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int TourId)
        {
            // 🔹 Lấy UserId từ Authentication Claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized(); // Người dùng chưa đăng nhập
            }

            int userId = int.Parse(userIdClaim); // Chuyển đổi từ string sang int

            // 🔹 Kiểm tra nếu Guide đã gửi yêu cầu trước đó
            var existingRequest = (await _tourGuideRepository.GetAllTourGuides())
                .FirstOrDefault(r => r.TourId == TourId && r.GuideId == userId);

            if (existingRequest == null)
            {
                var request = new TourGuide
                {
                    TourId = TourId,
                    GuideId = userId,
                    Status = "Pending",
                    RequestDate = System.DateTime.Now
                };

                await _tourGuideRepository.Add(request);
            }

            return RedirectToPage("/Guide/FixedTour/Tour");
        }

    }
}
