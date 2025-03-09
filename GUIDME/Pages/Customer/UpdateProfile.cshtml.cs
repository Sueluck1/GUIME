using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Security.Claims;

namespace GUIDME.Pages.Customer
{
    public class UpdateProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly string _imagePath = "/Images/User/";

        public UpdateProfileModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public User UserProfile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Authentication/Login");
            }

            UserProfile = await _userRepository.GetUserById(int.Parse(userId));

            if (UserProfile == null)
            {
                return RedirectToPage("/Authentication/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Authentication/Login");
            }

            var existingUser = await _userRepository.GetUserById(int.Parse(userId));
            if (existingUser == null)
            {
                return RedirectToPage("/Authentication/Login");
            }

            // Cập nhật thông tin cơ bản
            existingUser.Name = UserProfile.Name;
            existingUser.Username = UserProfile.Username;
            existingUser.Email = UserProfile.Email;
            existingUser.Mobile = UserProfile.Mobile;
            existingUser.Address = UserProfile.Address;
            existingUser.Gender = UserProfile.Gender;
            existingUser.DateOfBirth = UserProfile.DateOfBirth;

            // Xử lý upload ảnh đại diện
            if (file != null && file.Length > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = $"{existingUser.UserId}{fileExtension}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "User", fileName);

                // Lưu file vào thư mục wwwroot
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn ảnh trong database
                existingUser.ImageUrl = $"{_imagePath}{fileName}";
            }

            await _userRepository.Update(existingUser);
            TempData["SuccessMessage"] = "Thông tin cá nhân đã được cập nhật thành công!";
            return RedirectToPage("/Customer/Profile");
        }
    }
}
