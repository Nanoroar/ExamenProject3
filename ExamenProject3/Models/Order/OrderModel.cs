namespace ExamenProject3.Models.Order
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;


        public string Address { get; set; } = null!;

        public DateTime OrderDate { get; set; }
      
        public DateTime DueDate { get; set; }
        
        public string OrderStatus { get; set; } = null!;

    }
}
