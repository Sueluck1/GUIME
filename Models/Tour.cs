using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Tour
    {
        [Key]
        public int TourId { get; set; }

        [Required]
        public int CategoryId { get; set; } // Khóa ngoại liên kết với Category
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        public string Name { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; } // Địa điểm của tour
        public enum TourType { Fixed, Custom }
        public TourType Type { get; set; } = TourType.Fixed; // Phân biệt loại tour

        // 🔹 Dành cho FixedTour
        public decimal? Price { get; set; } // Giá chỉ áp dụng nếu là FixedTour

        // 🔹 Dành cho CustomTour
        public int? GuideId { get; set; }
        [ForeignKey("GuideId")]
        public User? Guide { get; set; }
        


        public decimal? CustomPrice { get; set; } // Giá chỉ áp dụng nếu là CustomTour
        public bool? IsApproved { get; set; } = false; // Duyệt tour nếu là CustomTour
        public bool IsActive { get; set; } = false;
        public int MaxParticipants { get; set; }  // Số lượng người tham gia tối đa
        public string Schedule { get; set; }  // Lịch trình tour với các mốc thời gian và hoạt động
        public string? TransportMethod { get; set; }  // Phương tiện di chuyển (xe bus, tàu hỏa, máy bay, v.v...)

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<TourImage> TourImages { get; set; } = new List<TourImage>();

    }
}
