using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecomm_web_api.Models.Entity
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }

        // 🔗 Relation with User
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        // 🔗 Relation with Medicine
        [Required]
        public Guid MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        public int UnitPrice { get; set; }   // capture price at time of adding to cart

        [Range(0, 100)]
        public int Discount { get; set; }    // capture discount at time of adding to cart

        // Computed property - not mapped to DB
        [NotMapped]
        public decimal TotalPrice => (UnitPrice - (UnitPrice * Discount / 100m)) * Quantity;
    }
}
