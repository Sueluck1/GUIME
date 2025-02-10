using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required]
        public int ?UserId { get; set; }  // Người gửi phản hồi

        [Required]
        public int ?TourId { get; set; }  // Tour liên quan đến phản hồi

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }  // Nội dung phản hồi
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("TourId")]
        public Tour Tour { get; set; }
    }
}
