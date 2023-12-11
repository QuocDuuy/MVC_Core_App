using Demo_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo_Project.Models
{
    public class OrderDetail
    {
        [Key]
        public int id { get; set; }

        public int product_id { get; set; }  // Use this property for the ProductId foreign key

        [ForeignKey("product_id")]
        public Product Product { get; set; }

        public int orderId { get; set; }  // Use this property for the OrderId foreign key

        [ForeignKey("orderId")]
        public OrderInfor Order_Infor { get; set; }

        [Required]
        public decimal total_price { get; set; }

        [Required]
        public int quantity { get; set; }

        [MaxLength(20)]
        [Required]
        public string order_status { get; set; }
        public DateTime cus_date { get; set; } = DateTime.Now;
    }
}
