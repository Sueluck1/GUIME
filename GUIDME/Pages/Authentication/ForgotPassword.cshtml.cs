using GUIDME.Pages.Authenthication.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace GUIDME.Pages.Authenthication
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordModel(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [BindProperty]
        public string Email { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            var user = await _userRepository.GetUserByEmail(Email);
            if (user == null)
            {
                Message = "Email không tồn tại trong hệ thống.";
                return Page();
            }


            var resetCode = Guid.NewGuid().ToString();
            user.VerificationCode = resetCode;

            await _userRepository.Update(user);


            var resetLink = Url.Page("/Authentication/ChangePassword",
                pageHandler: null,
                values: new { email = user.Email, code = resetCode },
                protocol: Request.Scheme);


            await _emailService.SendEmailAsync(user.Email, "Đặt lại mật khẩu",
                $"Nhấp vào liên kết này để đặt lại mật khẩu của bạn: <a href='{resetLink}'>Đặt lại mật khẩu</a>");

            Message = "A password reset link has been sent to your email.";
            return Page();
        }
    }
}
