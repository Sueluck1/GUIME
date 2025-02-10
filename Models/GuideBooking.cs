using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class GuideBooking
    {
        [Key]
        public int GuideBookingId { get; set; }

        [Required]
        public int UserId { get; set; }  // Người đặt hướng dẫn viên
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int GuideId { get; set; }  // Hướng dẫn viên được đặt
        [ForeignKey("GuideId")]
        public User Guide { get; set; }

        [Required]
        public DateTime BookingTime { get; set; } = DateTime.Now;  // Thời gian đặt

        [Required]
        public string Status { get; set; } = "Pending";  // Trạng thái (Pending, Accepted, Completed, Canceled)

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }  // Giá cho chuyến đi

        [MaxLength(255)]
        public string? Location { get; set; }  // Địa điểm hẹn hướng dẫn viên
    }
}
