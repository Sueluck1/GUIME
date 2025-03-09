using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace GUIDME.Pages.Customer.BookingRequest
{
    public class SuccessModel : PageModel
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentRepository _paymentRepository;

        public SuccessModel(IBookingRepository bookingRepository, IPaymentRepository paymentRepository)
        {
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
        }

        // Phương thức duy nhất xử lý yêu cầu GET với bookingId
        public async Task<IActionResult> OnGetAsync(int bookingId)
        {
            // Lấy thông tin booking
            var booking = await _bookingRepository.GetBookingById(bookingId);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đặt chỗ.";
                return RedirectToPage("/Customer/Profile");
            }

            // Cập nhật trạng thái của booking
            booking.Status = "Completed"; // Hoặc trạng thái thanh toán thành công
            await _bookingRepository.Update(booking);

            // Thêm thông tin vào bảng Payment
            var payment = new Payment
            {
                BookingId = booking.BookingId,
                Amount = booking.TotalPrice,
                PaymentDate = DateTime.Now,
                IsSuccessful = true, // Thanh toán thành công
                PaymentMethod = "PayOS", // Đặt phương thức thanh toán là PayOS
            };

            await _paymentRepository.Add(payment);

            // Hiển thị thông báo thành công
            TempData["SuccessMessage"] = "Thanh toán thành công! Đặt tour của bạn đã được xác nhận.";
            return Page();
        }
    }
}
