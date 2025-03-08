using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace GUIDME.Pages.Customer.FixedTour
{
    public class DetailModel : PageModel
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourImageRepository _tourImageRepository;

        public DetailModel(ITourRepository tourRepository, ITourImageRepository tourImageRepository)
        {
            _tourRepository = tourRepository;
            _tourImageRepository = tourImageRepository;
        }

        public TourDetailViewModel Tour { get; set; }
        public List<TourImage> TourImages { get; set; } = new List<TourImage>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var tour = await _tourRepository.GetTourById(id);
            if (tour == null || !tour.IsActive)
            {
                return NotFound();
            }

            Tour = new TourDetailViewModel
            {
                TourId = tour.TourId,
                Name = tour.Name,
                ThumbnailUrl = string.IsNullOrEmpty(tour.ThumbnailUrl) ? "/images/default-tour.jpg" : tour.ThumbnailUrl,
                Rating = tour.Ratings.Count > 0 ? (double)tour.Ratings.Average(r => r.Score) : 0.0,
                ReviewCount = tour.Ratings.Count,
                Duration = $"{(tour.EndDate - tour.StartDate).TotalHours} giờ",
                Price = tour.Price ?? 0,
                StartDate = tour.StartDate,
                EndDate = tour.EndDate,
                Description = tour.Description,
                Category = tour.Category,

                // Thêm các thuộc tính mới từ Tour
                MaxParticipants = tour.MaxParticipants, // Số lượng người tham gia tối đa
                Schedule = tour.Schedule, // Lịch trình tour
                TransportMethod = tour.TransportMethod // Phương tiện di chuyển
            };

            // Lấy danh sách ảnh của tour
            TourImages = (await _tourImageRepository.GetTourImagesByTourId(id)).ToList();

            return Page();
        }
    }

    public class TourDetailViewModel
    {
        public int TourId { get; set; }
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Category Category { get; set; }
        public int MaxParticipants { get; set; }
        public string Schedule { get; set; }
        public string TransportMethod { get; set; }
    }


}

