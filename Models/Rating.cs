using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [Required]
        public int ?UserId { get; set; }  // Người đánh giá

        [Required]
        public int ?TourId { get; set; }  // Tour được đánh giá

        [Range(1, 5)]
        public int Score { get; set; }  // Điểm đánh giá từ 1 đến 5

        [MaxLength(1000)]
        public string? Comment { get; set; }  // Bình luận của người dùng

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("TourId")]
        public Tour Tour { get; set; }
    }
}
