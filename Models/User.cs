using System;
using System.ComponentModel.DataAnnotations;

namespace Ecomm_web_api.Models.Entity
{
    public enum UserStatus
    {
        Active,
        Inactive,
        Suspended
    }

    public enum UserType
    {
        Customer,
        Admin,
        Seller
    }

    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        public DateOnly? DOB { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public UserStatus Status { get; set; } = UserStatus.Active;

        [Required]
        public UserType Type { get; set; } = UserType.Customer;

        [Range(0, int.MaxValue)]
        public int Fund { get; set; } = 0;
    }
}
