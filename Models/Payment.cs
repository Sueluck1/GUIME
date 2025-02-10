using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int BookingId { get; set; }  // Liên kết với đặt tour

        [Required]
        public decimal Amount { get; set; }  // Số tiền thanh toán

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }  // Phương thức thanh toán (Visa, Momo, PayPal,...)

        public bool IsSuccessful { get; set; } = false;  // Thanh toán có thành công không

        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}
