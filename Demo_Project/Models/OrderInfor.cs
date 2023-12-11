using System;
using System.ComponentModel.DataAnnotations;

namespace Demo_Project.Models
{
    public class OrderInfor
    {
        [Key]
        public int order_id { get; set; }

        [Required]
        [MaxLength(50)]
        public string name { get; set; }

        [Required]
        [MaxLength(50)]
        public string email { get; set; }

        [Required]
        [MaxLength(10)]
        public string phone { get; set; }

        [Required]
        [MaxLength(50)]
        public string cus_address { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
