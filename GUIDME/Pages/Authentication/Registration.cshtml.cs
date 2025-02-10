using GUIDME.Pages.Authenthication.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace GUIDME.Pages.Authenthication
{
    public class RegistrationModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public RegistrationModel(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [BindProperty]
        public User NewUser { get; set; } = new User();

        public string Message { get; private set; }
        public async Task<IActionResult> OnPostRegisterAsync()
        {

            var existingUser = await _userRepository.GetUserByEmail(NewUser.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("NewUser.Email", "Email này đã được sử dụng. Vui lòng chọn email khác.");
                return Page();
            }
            NewUser.VerificationCode = Guid.NewGuid().ToString();

            try
            {
                NewUser.Role = "User";
                await _userRepository.Add(NewUser);
                var confirmationLink = Url.Page("/Authentication/ConfirmEmail",
                                   pageHandler: null,
                                   values: new { email = NewUser.Email, code = NewUser.VerificationCode },
                                   protocol: Request.Scheme);

                await _emailService.SendEmailAsync(NewUser.Email, "Email Confirmation",
                                 $"Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau: <a href='{confirmationLink}'>link</a>");
                Message = "Đăng ký thành công! Vui lòng kiểm tra email của bạn để xác nhận tài khoản.";
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại.");
                return Page();
            }
        }
    }
}
