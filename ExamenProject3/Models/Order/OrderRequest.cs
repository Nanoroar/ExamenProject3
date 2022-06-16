namespace ExamenProject3.Models.Order
{
    public class OrderRequest
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;
       
        public string Address { get; set; } = null!;
        public DateTime OrderDate { get; set; }= DateTime.Now;  
      
        public DateTime DueDate { get; set; }=DateTime.Now.AddDays(1);
    
        public string OrderStatus { get; set; } = null!;

    }
}
