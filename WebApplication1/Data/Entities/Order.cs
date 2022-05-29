using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderRows = new HashSet<OrderRow>();
        }

        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public string OrderStatus { get; set; } = null!;

        [InverseProperty("Order")]
        public virtual ICollection<OrderRow> OrderRows { get; set; }
    }
}
