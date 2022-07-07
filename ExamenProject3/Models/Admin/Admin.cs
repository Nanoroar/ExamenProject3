﻿namespace ExamenProject3.Models.Admin
{
    public class Admin
    {
        public int Id { get; set; } 
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
        public string Role { get; set; } = "Admin";
    }
}
