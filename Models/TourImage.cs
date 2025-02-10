using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TourImage
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public int TourId { get; set; }  // Khóa ngoại liên kết với Tour

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } // Đường dẫn ảnh

        [ForeignKey("TourId")]
        public Tour Tour { get; set; }
    }
}
