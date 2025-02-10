using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;

namespace GUIDME.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly ITourRepository _tourRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentRepository _paymentRepository;

        public int UserCount { get; set; }
        public int TourCount { get; set; }
        public int BookingCount { get; set; }
        public int PaymentCount { get; set; }

        public List<BookingViewModel> RecentBookings { get; set; } = new List<BookingViewModel>();

        public DashboardModel(
            IUserRepository userRepository,
            ITourRepository tourRepository,
            IBookingRepository bookingRepository,
            IPaymentRepository paymentRepository)
        {
            _userRepository = userRepository;
            _tourRepository = tourRepository;
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task OnGetAsync()
        {
            UserCount = await _userRepository.GetUserCount();
            TourCount = await _tourRepository.GetTourCount();
            BookingCount = await _bookingRepository.GetBookingCount();
            PaymentCount = await _paymentRepository.GetPaymentCount();

            var bookings = await _bookingRepository.GetAllBookings();
            foreach (var booking in bookings)
            {
                RecentBookings.Add(new BookingViewModel
                {
                    BookingId = booking.BookingId,
                    UserName = booking.User.Name,
                    TourName = booking.Tour.Name,
                    BookingDate = booking.BookingDate,
                    Status = booking.Status
                });
            }
        }
    }

    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public string UserName { get; set; }
        public string TourName { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
    }
}
