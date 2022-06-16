using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamenProject3.Models.Order
{
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
