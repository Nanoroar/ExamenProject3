namespace ExamenProject3.Models.User
{
    public class UserUpdate
    {
        
        public string PhoneNumber { get; set; } = null!;

        public int UserAddressId { get; set; }

        public string StreetName { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
