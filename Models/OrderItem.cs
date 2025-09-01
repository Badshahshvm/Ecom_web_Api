using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecomm_web_api.Models.Entity
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }   // Auto-generated PK

        [Required]
        public Guid OrderId { get; set; }   // FK to Order table
        public Order Order { get; set; }

        [Required]
        public Guid MedicineId { get; set; }   // FK to Medicine table
        public Medicine Medicine { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int UnitPrice { get; set; }

        [Range(0, 100)]
        public int Discount { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        // Computed property - not mapped to DB
        [NotMapped]
        public decimal TotalPrice => (UnitPrice - (UnitPrice * Discount / 100m)) * Quantity;
    }
}
