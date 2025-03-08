using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace GUIDME.Pages.Admin
{
    public class AdminTourGuideRequestsModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public AdminTourGuideRequestsModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> UserRequests { get; set; }

        public async Task OnGetAsync()
        {
            UserRequests = (await _userRepository.GetAllUsers())
                            .Where(u => u.Role == "User" && u.IsRequest == true)
                            .ToList();
        }
    }
}
