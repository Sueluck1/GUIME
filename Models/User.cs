using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? ImageUrlCCCDFront { get; set; } 

        [MaxLength(500)]
        public string? ImageUrlCCCDBack { get; set; }
       

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public int? ExperienceYears { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [MaxLength(15)]
        public string? Mobile { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        public string? Address { get; set; }
        [Required]
        public string? Password { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string? Role { get; set; }

        public string? VerificationCode { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsRequest { get; set; } = false;
        public ICollection<Tour> Tours { get; set; } // Quan hệ với Tour
        public ICollection<Booking> Bookings { get; set; } // Quan hệ với Booking
        public ICollection<Feedback> Feedbacks { get; set; } // Quan hệ với Feedback
        public ICollection<Rating> Ratings { get; set; } // Quan hệ với Rating
        public ICollection<Certificate> Certificates { get; set; }
    }
}
