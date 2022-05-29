using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data.Entities
{
    [Index("UserAddressId", Name = "IX_Users_UserAddressId")]
    public partial class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public byte[] Hash { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
        public int UserAddressId { get; set; }

        [ForeignKey("UserAddressId")]
        [InverseProperty("Users")]
        public virtual Address UserAddress { get; set; } = null!;
    }
}
