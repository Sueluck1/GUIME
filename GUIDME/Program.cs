using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Models;
using GUIDME.Pages.Authenthication.Service;
using Microsoft.Extensions.Options;

namespace GUIDME
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Thêm các dịch vụ vào container
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();

            // Cấu hình DbContext
            builder.Services.AddDbContext<GuidmeDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Thêm Repository
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ITourRepository, TourRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();
            builder.Services.AddScoped<ITourImageRepository, TourImageRepository>();
            builder.Services.AddScoped<ITourGuideRepository, TourGuideRepository>();
            builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
            // Thêm các dịch vụ
            builder.Services.AddScoped<IEmailService, EmailService>();
            

            // Cấu hình xác thực Cookie
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Users/Login";
                    options.AccessDeniedPath = "/Users/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    options.SlidingExpiration = true;  // Reset thời gian hết hạn mỗi khi người dùng hoạt động
                });
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(8080); // Luôn lắng nghe trên cổng 8080
              

            });
            var app = builder.Build();

            // Cấu hình pipeline xử lý yêu cầu HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

           

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            Console.WriteLine($"App is listening on port: {port}");
            app.UseRouting();
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                var logger = app.Services.GetService<ILogger<Program>>();
                var addresses = app.Urls;
                foreach (var address in addresses)
                {
                    logger.LogInformation($"Ứng dụng đang chạy trên: {address}");
                }
            });
            app.UseAuthentication();
            app.UseAuthorization();

           


            app.MapRazorPages();
            

            app.Run();
        }
    }
}
