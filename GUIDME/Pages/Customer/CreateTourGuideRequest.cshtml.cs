using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace GUIDME.Pages.Customer
{
    public class CreateTourGuideRequestModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly ICertificateRepository _certificateRepository;

        public CreateTourGuideRequestModel(IUserRepository userRepository, ICertificateRepository certificateRepository)
        {
            _userRepository = userRepository;
            _certificateRepository = certificateRepository;
        }

        [BindProperty]
        public IFormFile CCCDFrontFile { get; set; }

        [BindProperty]
        public IFormFile CCCDBackFile { get; set; }

        [BindProperty]
        public IFormFile[] CertificateFiles { get; set; } // Sử dụng mảng để xử lý nhiều tệp

        [BindProperty]
        public string CertificateNames { get; set; } // Nhận tên của chứng chỉ

        public void OnGet()
        {
            // Hiển thị trang đăng ký yêu cầu
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Authentication/Login");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var existingUser = await _userRepository.GetUserById(int.Parse(userId));

            // Kiểm tra nếu yêu cầu đã được gửi đi và đang chờ duyệt
            if (existingUser != null && existingUser.IsRequest)
            {
                // Nếu yêu cầu đã được gửi, hiển thị thông báo yêu cầu đang duyệt
                TempData["ErrorMessage"] = "Your request is currently under review. Please wait until it is approved.";
                return RedirectToPage("/Customer/Profile"); // Hoặc trang bạn muốn hiển thị thông báo
            }

            // Lưu ảnh CCCD và chứng chỉ nếu có
            string cccdFrontPath = null;
            string cccdBackPath = null;

            // Lưu ảnh mặt trước CCCD
            if (CCCDFrontFile != null)
            {
                var cccdFrontFileName = $"{Guid.NewGuid()}_{CCCDFrontFile.FileName}";
                cccdFrontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/cccd_front", cccdFrontFileName);

                using (var stream = new FileStream(cccdFrontPath, FileMode.Create))
                {
                    await CCCDFrontFile.CopyToAsync(stream);
                }
            }

            // Lưu ảnh mặt sau CCCD
            if (CCCDBackFile != null)
            {
                var cccdBackFileName = $"{Guid.NewGuid()}_{CCCDBackFile.FileName}";
                cccdBackPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/cccd_back", cccdBackFileName);

                using (var stream = new FileStream(cccdBackPath, FileMode.Create))
                {
                    await CCCDBackFile.CopyToAsync(stream);
                }
            }

            // Lưu nhiều chứng chỉ
            if (CertificateFiles != null && CertificateFiles.Length > 0)
            {
                var names = CertificateNames.Split(','); // Tách tên chứng chỉ từ chuỗi nhập

                // Kiểm tra xem số lượng chứng chỉ và tên có phù hợp không
                if (names.Length != CertificateFiles.Length)
                {
                    // Nếu không khớp, trả về lỗi hoặc thông báo
                    ModelState.AddModelError("CertificateNames", "Số lượng chứng chỉ và tên chứng chỉ không khớp.");
                    return Page();
                }

                var filePaths = new List<string>();

                for (int i = 0; i < CertificateFiles.Length; i++)
                {
                    var certificateFileName = $"{Guid.NewGuid()}_{CertificateFiles[i].FileName}";
                    var certificateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/certificates", certificateFileName);

                    using (var stream = new FileStream(certificateFilePath, FileMode.Create))
                    {
                        await CertificateFiles[i].CopyToAsync(stream);
                    }

                    filePaths.Add($"/uploads/certificates/{certificateFileName}");
                }

                // Lưu chứng chỉ vào bảng Certificates
                for (int i = 0; i < names.Length; i++)
                {
                    var certificate = new Certificate
                    {
                        CertificateName = names[i].Trim(), // Lưu tên chứng chỉ
                        CertificateImageUrl = filePaths.ElementAtOrDefault(i), // Đường dẫn ảnh chứng chỉ
                        UserId = int.Parse(userId) // Lưu UserId
                    };

                    await _certificateRepository.Add(certificate);
                }
            }

            // Cập nhật User với thông tin đã tải lên
            if (existingUser != null)
            {
                existingUser.IsRequest = true; // Đánh dấu yêu cầu là đã gửi đi

                existingUser.ImageUrlCCCDFront = $"/uploads/cccd_front/{Path.GetFileName(cccdFrontPath)}";
                existingUser.ImageUrlCCCDBack = $"/uploads/cccd_back/{Path.GetFileName(cccdBackPath)}";

                await _userRepository.Update(existingUser);
            }

            TempData["SuccessMessage"] = "Successful Registration as a Tour Guide";
            return RedirectToPage("/Customer/Profile");
        }

    }
}
