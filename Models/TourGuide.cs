using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TourGuide
    {
        [Key]
        public int TourGuideId { get; set; }

        [Required]
        public int? TourId { get; set; }  // Liên kết với FixedTour

        [Required]
        public int? GuideId { get; set; }  // Liên kết với User (Hướng dẫn viên)

        [ForeignKey("TourId")]
        public Tour Tour { get; set; }

        [ForeignKey("GuideId")]
        public User Guide { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Trạng thái yêu cầu: Pending, Approved, Rejected

        public DateTime RequestDate { get; set; } = DateTime.Now; // Ngày gửi yêu cầu
    }
}
