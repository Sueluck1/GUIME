using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUIDME.Pages.Guide.FixedTour
{
    public class TourModel : PageModel
    {
        private readonly ITourRepository _tourRepository;

        public List<Tour> Tours { get; set; } = new List<Tour>();

        public TourModel(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public async Task OnGetAsync()
        {
            Tours = (await _tourRepository.GetAllTours()).Where(t => !t.IsActive).ToList();
        }
    }
}
