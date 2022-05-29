using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data.Entities
{
    public partial class OrderRow
    {
        [Key]
        public int OrderId { get; set; }
        [Key]
        public string ArticaleNumber { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        [Column(TypeName = "money")]
        public decimal ProductPrice { get; set; }

        [ForeignKey("OrderId")]
        [InverseProperty("OrderRows")]
        public virtual Order Order { get; set; } = null!;
    }
}
