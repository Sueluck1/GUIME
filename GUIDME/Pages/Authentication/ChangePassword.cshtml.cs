using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace GUIDME.Pages.Authenthication
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public ChangePasswordModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string code)
        {
            var user = await _userRepository.GetUserByEmail(email);

            // Kiểm tra mã xác thực
            if (user == null || user.VerificationCode != code)
            {
                Message = "Mã xác thực không hợp lệ hoặc đã hết hạn.";
                return Page();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string email, string code)
        {
            if (NewPassword != ConfirmPassword)
            {
                Message = "Mật khẩu nhập lại không khớp.";
                return Page();
            }

            var user = await _userRepository.GetUserByEmail(email);

            if (user == null || user.VerificationCode != code)
            {
                Message = "Mã xác thực không hợp lệ hoặc đã hết hạn.";
                return Page();
            }

            // Cập nhật mật khẩu người dùng
            user.Password = NewPassword;
            user.VerificationCode = null; // Đặt lại mã xác thực

            await _userRepository.Update(user);

            Message = "Mật khẩu đã được cập nhật thành công.";
            return RedirectToPage("/Authentication/Login");
        }
    }

}
