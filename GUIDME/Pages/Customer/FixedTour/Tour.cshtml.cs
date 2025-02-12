using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUIDME.Pages.Customer.FixedTour
{
    public class TourModel : PageModel
    {
        private readonly ITourRepository _tourRepository;

        public TourModel(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public List<TourViewModel> Tours { get; set; } = new List<TourViewModel>();

        public async Task OnGetAsync()
        {
            var tours = await _tourRepository.GetAllTours();

            // Lọc danh sách tour chỉ lấy những tour có IsActive = true
            var activeTours = tours.Where(t => t.IsActive).ToList();

            foreach (var tour in activeTours)
            {
                Tours.Add(new TourViewModel
                {
                    TourId = tour.TourId,
                    Name = tour.Name,
                    ThumbnailUrl = string.IsNullOrEmpty(tour.ThumbnailUrl) ? "/images/default-tour.jpg" : tour.ThumbnailUrl,
                    Rating = tour.Ratings.Count > 0 ? (double)tour.Ratings.Average(r => r.Score) : 0.0,
                    ReviewCount = tour.Ratings.Count,
                    Duration = $"{(tour.EndDate - tour.StartDate).TotalHours} giờ",
                    Price = tour.Price ?? 0,
                    IsPopular = tour.Ratings.Count > 5000 // Giả định nếu có hơn 5000 review thì là phổ biến
                });
            }
        }
    }

    public class TourViewModel
    {
        public int TourId { get; set; }
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }
        public bool IsPopular { get; set; }
    }
}
