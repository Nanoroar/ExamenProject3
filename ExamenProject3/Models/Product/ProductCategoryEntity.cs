using System.ComponentModel.DataAnnotations;

namespace ExamenProject3.Models.Product
{
    public class ProductCategoryEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public  string CategoryName { get; set; } = null!;

        public virtual ICollection<ProductEntity> Products { get; set; } = null!;

        
    }
}
