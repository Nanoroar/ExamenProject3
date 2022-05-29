using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamenProject3.Models.Order
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }
        public int  CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public DateTime  OrderDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public string OrderStatus { get; set; } = null!;

        public virtual List<OrderRowEntity> OrderRows { get; set; }= null!;




    }


    public class OrderRowEntity
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string ArticaleNumber { get; set; } = null!;
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName="money")]
        public decimal ProductPrice { get; set; }

        public OrderEntity Order { get; set; } = null!;
    }
}
