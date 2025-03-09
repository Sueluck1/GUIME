using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Net.payOS;
using System.Threading.Tasks;
using Net.payOS.Types;
using System.Security.Claims;

namespace GUIDME.Pages.Customer
{
    public class ProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly PayOS _payOS;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileModel(IUserRepository userRepository, IBookingRepository bookingRepository, PayOS payOS, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _payOS = payOS;
            _httpContextAccessor = httpContextAccessor;
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

        // Xử lý thanh toán
        public async Task<IActionResult> OnPostProcessPaymentAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingById(bookingId);
            if (booking == null || booking.Status != "Pending")
            {
                TempData["ErrorMessage"] = "Không tìm thấy đặt chỗ hoặc đặt chỗ đã được xử lý.";
                return RedirectToPage();
            }

            // Gọi API thanh toán (ví dụ PayOS)
            var paymentUrl = await CreatePaymentLink(booking);

            if (paymentUrl != null)
            {
               
                return Redirect(paymentUrl);  // Chuyển hướng đến trang thanh toán
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình thanh toán.";
                return RedirectToPage();
            }
        }
        public async Task<IActionResult> OnPostCancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingById(bookingId);
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đặt chỗ.";
                return RedirectToPage();
            }

            // Xóa booking
            await _bookingRepository.Delete(bookingId);
            TempData["SuccessMessage"] = "Đặt chỗ đã bị hủy thành công.";
            return RedirectToPage();
        }
        private async Task<string> CreatePaymentLink(Booking booking)
        {
            long orderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var items = new List<ItemData>
            {
                new ItemData(booking.Tour.Name, 1, (int)booking.Tour.Price)
            };

            // Lấy base URL của ứng dụng để xử lý callback
            var httpRequest = _httpContextAccessor.HttpContext?.Request;
            if (httpRequest == null)
            {
                return null;
            }

            var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";
            string description = $"Thanh toán tour: {booking.Tour.Name}";
            if (description.Length > 25)
            {
                description = description.Substring(0, 25);
            }

            PaymentData paymentData = new PaymentData(
                orderCode,
                (int)booking.Tour.Price * booking.NumberOfPeople,
                description,
                items,
                $"{baseUrl}/Customer/BookingRequest/Cancel",
                $"{baseUrl}/Customer/BookingRequest/Success?bookingId={booking.BookingId}"
            );

            var createPayment = await _payOS.createPaymentLink(paymentData);
            return createPayment.checkoutUrl;
        }
    }
}
