using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using System.Security.Claims;

namespace GUIDME.Pages.Authenthication
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public LoginModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [TempData]
        public string Message { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var user = await _userRepository.Login(Username, Password);
            if (user != null)
            {
                if (!user.IsEmailVerified)
                {
                    Message = "Your email is not verified yet. Please verify your email to log in.";
                    return Page();
                }


                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


                if (user.Role == "Admin")
                {
                    return RedirectToPage("/Admin/Dashboard");
                }
                else if (user.Role == "Guide")
                {
                    return RedirectToPage("/Guide/FixedTour/Tour"); // 🔹 Chuyển hướng đến trang dành cho hướng dẫn viên
                }
                else
                {
                    return RedirectToPage("/Customer/Index");
                }

            }

            // Nếu đăng nhập thất bại
            Message = "Invalid username or password";
            return Page();
        }
    }

}
