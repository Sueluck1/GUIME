using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GUIDME.Pages.Authenthication
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(); // Sign the user out of the application
            TempData["Message"] = "You have successfully logged out.";
            // Redirect the user to the login page
            return RedirectToPage("/Authentication/Login");
        }
    }
}
