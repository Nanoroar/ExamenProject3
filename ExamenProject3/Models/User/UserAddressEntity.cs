using System.ComponentModel.DataAnnotations;

namespace ExamenProject3.Models.User
{
    public class UserAddressEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StreetName { get; set; } = null!;
        [Required]
        public string PostalCode { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;

        public virtual List<UserEntity> Users { get; set; } = null!;

        public UserAddressEntity()
        {

        }

        public UserAddressEntity( string streetName, string postalCode, string city)
        {
          
            StreetName = streetName;
            PostalCode = postalCode;
            City = city;
           
        }
    }
}
