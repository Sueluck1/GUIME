using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace GUIDME.Pages.Admin.FixedTour
{
    public class TourModel : PageModel
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourImageRepository _tourImageRepository;
        private readonly ICategoryRepository _categoryRepository;

        public List<Tour> FixedTours { get; set; } = new List<Tour>();
        public List<Category> Categories { get; set; } = new List<Category>();

        [BindProperty]
        public Tour NewTour { get; set; } = new Tour();

        [BindProperty]
        public List<IFormFile> ImageFiles { get; set; }

        public TourModel(ITourRepository tourRepository,
                         ITourImageRepository tourImageRepository,
                         ICategoryRepository categoryRepository)
        {
            _tourRepository = tourRepository;
            _tourImageRepository = tourImageRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task OnGetAsync()
        {
            FixedTours = (await _tourRepository.GetAllTours()).ToList();
            Categories = (await _categoryRepository.GetAllCategories()).ToList(); // Lấy danh sách category
        }

        public async Task<IActionResult> OnPostAsync()
        {
           

            var newTour = new Tour
            {
                Name = Request.Form["Name"],
                CategoryId = int.Parse(Request.Form["CategoryId"]), // Lưu CategoryId
                Description = Request.Form["Description"],
                StartDate = DateTime.Parse(Request.Form["StartDate"]),
                EndDate = DateTime.Parse(Request.Form["EndDate"]),
                Price = decimal.Parse(Request.Form["Price"]),
                Type = Tour.TourType.Fixed
            };

            await _tourRepository.Add(newTour);

            // Lưu ảnh nếu có
            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                foreach (var image in ImageFiles)
                {
                    var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                    var filePath = Path.Combine("wwwroot/uploads/FixedTours", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var tourImage = new TourImage
                    {
                        TourId = newTour.TourId,
                        ImageUrl = $"/uploads/FixedTours/{fileName}"
                    };

                    await _tourImageRepository.Add(tourImage);
                }
            }

            return RedirectToPage("/Admin/FixedTour/Tour");
        }


        public async Task<IActionResult> OnGetToggleActiveAsync(int id)
        {
            var tour = await _tourRepository.GetTourById(id);
            if (tour == null)
            {
                return NotFound();
            }

            // Đảo trạng thái IsActive (Active -> Inactive, Inactive -> Active)
            tour.IsActive = !tour.IsActive;

            await _tourRepository.Update(tour);

            return RedirectToPage("/Admin/FixedTour/Tour");
        }

    }
}
