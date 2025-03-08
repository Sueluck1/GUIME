using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        [Required]
        [MaxLength(255)]
        public string CertificateName { get; set; }  // Tên chứng chỉ

        [MaxLength(500)]
        public string CertificateImageUrl { get; set; }  // Đường dẫn ảnh chứng chỉ

        // Thuộc tính khóa ngoại liên kết với User
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }  // Quan hệ với User
    }

}
