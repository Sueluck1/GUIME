using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GUIDME.Pages.Admin.FixedTour
{
    public class EditTourModel : PageModel
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourImageRepository _tourImageRepository;

        [BindProperty]
        public Tour Tour { get; set; }

        [BindProperty]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();

        [BindProperty]
        public IFormFile? ThumbnailFile { get; set; } // Ảnh đại diện của tour

        public List<TourImage> TourImages { get; set; } = new List<TourImage>();

        public EditTourModel(ITourRepository tourRepository, ITourImageRepository tourImageRepository)
        {
            _tourRepository = tourRepository;
            _tourImageRepository = tourImageRepository;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Tour = await _tourRepository.GetTourById(id);
            if (Tour == null)
            {
                return NotFound();
            }

            // Lấy danh sách ảnh hiện tại của Tour
            TourImages = (await _tourImageRepository.GetTourImagesByTourId(id)).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var existingTour = await _tourRepository.GetTourById(Tour.TourId);
            if (existingTour == null)
            {
                return NotFound();
            }

            // Update tour details
            existingTour.Name = Tour.Name;
            existingTour.Description = Tour.Description;
            existingTour.StartDate = Tour.StartDate;
            existingTour.EndDate = Tour.EndDate;
            existingTour.Price = Tour.Price;
            existingTour.MaxParticipants = Tour.MaxParticipants;  // New field
            existingTour.Schedule = Tour.Schedule;  // New field
            existingTour.TransportMethod = Tour.TransportMethod;  // New field

            // Handle thumbnail image if provided
            if (ThumbnailFile != null)
            {
                var thumbnailFileName = $"{Guid.NewGuid()}_{ThumbnailFile.FileName}";
                var thumbnailFilePath = Path.Combine("wwwroot/uploads/Thumbnails", thumbnailFileName);

                using (var stream = new FileStream(thumbnailFilePath, FileMode.Create))
                {
                    await ThumbnailFile.CopyToAsync(stream);
                }

                existingTour.ThumbnailUrl = $"/uploads/Thumbnails/{thumbnailFileName}";
            }

            await _tourRepository.Update(existingTour);

            // Handle new images if provided
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

                    var newImage = new TourImage
                    {
                        TourId = existingTour.TourId,
                        ImageUrl = $"/uploads/FixedTours/{fileName}"
                    };

                    await _tourImageRepository.Add(newImage);
                }
            }

            return RedirectToPage("/Admin/FixedTour/Tour");
        }

    }
}
