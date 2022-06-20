using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamenProject3.Models.Order
{
    public class OrderRowEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string ArticaleNumber { get; set; } = null!;
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName="money")]
        public decimal ProductPrice { get; set; }

        [Required]
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; } = null!;
    }
}
