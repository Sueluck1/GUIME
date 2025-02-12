using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Models
{
    public class GuidmeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<TourGuide> TourGuides { get; set; }
        public DbSet<TourImage> TourImages { get; set; }

        public GuidmeDbContext() { }
        public GuidmeDbContext(DbContextOptions<GuidmeDbContext> options) : base(options)
        {
            SeedData();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsetting.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==================== 1. Chỉ định kiểu dữ liệu Decimal ==================== 
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2); // Định dạng decimal(18,2)

            modelBuilder.Entity<Tour>()
                .Property(t => t.Price)
                .HasPrecision(18, 2); // Định dạng decimal(18,2)

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasPrecision(18, 2); // Định dạng decimal(18,2) để tránh bị mất giá trị



            // ==================== 2. Thiết lập Quan hệ 1-Nhiều ====================

            // 🌐 Quan hệ giữa User và Booking (1 User có nhiều Booking)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // 🌐 Quan hệ giữa Tour và Booking (1 Tour có nhiều Booking)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Tour)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TourId)
                .OnDelete(DeleteBehavior.NoAction);

            // 🌐 Quan hệ giữa Tour và Feedback (1 Tour có nhiều Feedback)
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Tour)
                .WithMany(t => t.Feedbacks)
                .HasForeignKey(f => f.TourId)
              .OnDelete(DeleteBehavior.NoAction);

            // 🌐 Quan hệ giữa User và Feedback (1 User có thể gửi nhiều Feedback)
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.NoAction);
            // 🌐 Quan hệ giữa Tour và Rating (1 Tour có nhiều Rating)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Tour)
                .WithMany(t => t.Ratings)
                .HasForeignKey(r => r.TourId)
                .OnDelete(DeleteBehavior.NoAction);

            // 🌐 Quan hệ giữa User và Rating (1 User có nhiều Rating)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // 🌐 Quan hệ giữa Tour và Category (1 Category có nhiều Tour)
            modelBuilder.Entity<Tour>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tours)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            // Quan hệ giữa CustomTour và User (1 Hướng dẫn viên có thể tạo nhiều CustomTour)
            modelBuilder.Entity<Tour>()
                .HasOne(t => t.Guide)
                .WithMany()
                .HasForeignKey(t => t.GuideId)
                .OnDelete(DeleteBehavior.SetNull);
            // ==================== 3. Quan hệ 1-1 ====================

            // 🌐 Quan hệ 1-1 giữa Booking và Payment
            modelBuilder.Entity<Payment>()
                .HasOne<Booking>()
                .WithOne()
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==================== 4. Quan hệ Nhiều-Nhiều ====================

            // 🌐 Quan hệ nhiều-nhiều giữa Tour và Guide (thông qua TourGuide)
            modelBuilder.Entity<TourGuide>()
                .HasOne(tg => tg.Tour)
                .WithMany()
                .HasForeignKey(tg => tg.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TourGuide>()
                .HasOne(tg => tg.Guide)
                .WithMany()
                .HasForeignKey(tg => tg.GuideId)
                .OnDelete(DeleteBehavior.NoAction);

            // ==================== 5. Sửa lỗi Shadow Property (Foreign Key ẩn) ====================

            // 🌐 Đảm bảo không có khóa ngoại ẩn trong Rating
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .HasConstraintName("FK_Rating_User");

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Tour)
                .WithMany(t => t.Ratings)
                .HasForeignKey(r => r.TourId)
                .HasConstraintName("FK_Rating_Tour");

            // 🌐 Đảm bảo không có khóa ngoại ẩn trong Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .HasConstraintName("FK_Booking_User");

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Tour)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TourId)
                .HasConstraintName("FK_Booking_Tour");

            // 🌐 Đảm bảo không có khóa ngoại ẩn trong Tour
            modelBuilder.Entity<Tour>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tours)
                .HasForeignKey(t => t.CategoryId)
                .HasConstraintName("FK_Tour_Category");

            modelBuilder.Entity<TourImage>()
           .HasOne(ti => ti.Tour)
           .WithMany(t => t.TourImages)
           .HasForeignKey(ti => ti.TourId)
           .OnDelete(DeleteBehavior.Cascade);

        }
        public void SeedData()
        {
            if (!Users.Any()) // Kiểm tra nếu chưa có dữ liệu
            {
                Users.AddRange(new List<User>
        {
            new User { Name = "Nguyen Van A", Username = "nguyenvana", Password = "password123", Email = "a@example.com", IsDeleted = false },
            new User { Name = "Tran Thi B", Username = "tranthib", Password = "password123", Email = "b@example.com", IsDeleted = false }
        });
            }

            if (!Categories.Any())
            {
                Categories.AddRange(new List<Category>
        {
            new Category { Name = "Du lịch biển", ImageUrl = "beach.jpg", IsActive = true },
            new Category { Name = "Du lịch núi", ImageUrl = "mountain.jpg", IsActive = true }
        });
            }

            if (!Tours.Any())
            {
                Tours.AddRange(new List<Tour>
        {
            new Tour { Name = "Tour Nha Trang 3 ngày", CategoryId = 1, Price = 5000000, IsActive = false },
            new Tour { Name = "Tour Sapa 5 ngày", CategoryId = 2, Price = 7000000, IsActive = false }
        });
            }

            if (!Bookings.Any())
            {
                Bookings.AddRange(new List<Booking>
        {
            new Booking { UserId = 1, TourId = 1, BookingDate = DateTime.Now, TourDate = DateTime.Now.AddDays(7), NumberOfPeople = 2, TotalPrice = 10000000, IsDeleted = false },
            new Booking { UserId = 2, TourId = 2, BookingDate = DateTime.Now, TourDate = DateTime.Now.AddDays(10), NumberOfPeople = 1, TotalPrice = 7000000, IsDeleted = false }
        });
            }

            if (!Ratings.Any())
            {
                Ratings.AddRange(new List<Rating>
        {
            new Rating { UserId = 1, TourId = 1, Score = 5, Comment = "Rất tốt!", IsDeleted = false },
            new Rating { UserId = 2, TourId = 2, Score = 4, Comment = "Dịch vụ ổn.", IsDeleted = false }
        });
            }

            if (!Feedbacks.Any())
            {
                Feedbacks.AddRange(new List<Feedback>
        {
            new Feedback { UserId = 1, TourId = 1, Content = "Hướng dẫn viên nhiệt tình.", IsDeleted = false },
            new Feedback { UserId = 2, TourId = 2, Content = "Cảnh đẹp, giá hợp lý.", IsDeleted = false }
        });
            }

            SaveChanges(); // Lưu thay đổi vào database
        }



    }
}
