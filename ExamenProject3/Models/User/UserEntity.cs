using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace ExamenProject3.Models.User
{
    public class UserEntity
    {
        public UserEntity()
        {

        }

        public UserEntity( string firstName, string lastName, string email, string phoneNumber, int userAddressId)
        {
       
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            
            UserAddressId = userAddressId;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]

        public byte[] Hash { get;private set; } = null!;
        [Required]

        public byte[] Salt { get;private set; } = null!;
        [Required]

        public int UserAddressId { get; set; }

        public virtual UserAddressEntity UserAddress { get; set; } = null!;

        public void CreateSecurityPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {

                Salt = hmac.Key;
                Hash= hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
                                  
        }

        public bool ComapareSecurePassword( string password)
        {
            bool result = false;
            using(var hmac = new HMACSHA512(Salt))
            {
                var _hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < _hash.Length; i++)
                    if (_hash[i] != Hash[i])
                        result = false;
                    else result = true;
            }
            return result;
        }
    }
}
