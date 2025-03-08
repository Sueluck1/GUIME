using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace GUIDME.Pages.Admin
{
    public class TourGuideDetailModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly ICertificateRepository _certificateRepository;

        public User User { get; set; }

        public TourGuideDetailModel(IUserRepository userRepository, ICertificateRepository certificateRepository)
        {
            _userRepository = userRepository;
            _certificateRepository = certificateRepository;
        }

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            // Lấy thông tin người dùng cùng với chứng chỉ
            User = await _userRepository.GetUserById(userId);

            if (User == null)
            {
                return NotFound();
            }

            // Bao gồm thông tin chứng chỉ
            User.Certificates = (ICollection<Certificate>)await _certificateRepository.GetCertificatesByUserId(userId);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int userId, string action)
        {
            var existingUser = await _userRepository.GetUserById(userId);
            if (existingUser == null)
            {
                return NotFound();
            }

           
            if (action == "approve")
            {
                existingUser.Role = "Guide";
            }
            // Từ chối yêu cầu: đặt IsRequest = false
            else if (action == "reject")
            {
                existingUser.IsRequest = false;
            }

            await _userRepository.Update(existingUser);

            return RedirectToPage("/Admin/AdminTourGuideRequests"); // Quay lại danh sách yêu cầu
        }
    }
}
