namespace ExamenProject3.Models.User
{
    public class AddressModel
    {
        public string StreetName { get; set; } = null!;
   
        public string PostalCode { get; set; } = null!;
       
        public string City { get; set; } = null!;


        public AddressModel()
        {
                
        }

        public AddressModel(string streetName, string postalCode, string city)
        {
            StreetName = streetName;
            PostalCode = postalCode;
            City = city;
        }
    }
}
