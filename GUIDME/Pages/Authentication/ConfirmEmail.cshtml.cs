using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace GUIDME.Pages.Authenthication
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public ConfirmEmailModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool IsConfirmed { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string code)
        {

            var user = await _userRepository.GetUserByEmail(email);
            if (user != null && user.VerificationCode == code)
            {

                user.IsEmailVerified = true;
                await _userRepository.Update(user);

                IsConfirmed = true;
            }
            else
            {
                IsConfirmed = false;
            }

            return Page();
        }
    }
}
