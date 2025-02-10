using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int ?UserId { get; set; }  // Người đặt tour

        [Required]
        public int ?TourId { get; set; }  // Tour được đặt

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime TourDate { get; set; }  // Ngày đi tour

        [Required]
        public int NumberOfPeople { get; set; }  // Số người tham gia
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }  // Tổng tiền

        [MaxLength(255)]
        public string? Status { get; set; } = "Pending";  // Trạng thái đặt chỗ (Pending, Confirmed, Cancelled)
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("TourId")]
        public Tour Tour { get; set; }
    }
}
