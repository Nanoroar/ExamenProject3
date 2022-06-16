using System.ComponentModel.DataAnnotations;

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
}
