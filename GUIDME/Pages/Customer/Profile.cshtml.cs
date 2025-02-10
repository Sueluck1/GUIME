using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Security.Claims;

namespace GUIDME.Pages.Customer
{
    
    
        /*[Authorize(Roles = "User")]*/
        public class ProfileModel : PageModel
        {
            private readonly IUserRepository _userRepository;
            private readonly IBookingRepository _bookingRepository;

            public ProfileModel(IUserRepository userRepository, IBookingRepository bookingRepository)
            {
                _userRepository = userRepository;
                _bookingRepository = bookingRepository;
            }

            public User UserProfile { get; set; }
            public List<Booking> BookingHistory { get; set; }

            public async Task<IActionResult> OnGetAsync()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Lấy thông tin người dùng
                UserProfile = await _userRepository.GetUserById(int.Parse(userId));

                if (UserProfile != null)
                {
                    // Lấy lịch sử đặt tour
                    BookingHistory = await _bookingRepository.GetBookingsByUserId(int.Parse(userId));
                }
                else
                {
                    return RedirectToPage("/Users/Login");
                }

                return Page();
            }
        }
    }
