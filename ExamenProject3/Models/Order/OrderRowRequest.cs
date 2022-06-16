namespace ExamenProject3.Models.Order
{
    public class OrderRowRequest
    {
        
        public int OrderId { get; set; }
        
        public string ArticaleNumber { get; set; } = null!;
     
        public string ProductName { get; set; } = null!;
       
        public int Quantity { get; set; }
       
        public decimal ProductPrice { get; set; }

    }
}
