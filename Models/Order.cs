using System;

using System.ComponentModel.DataAnnotations;

namespace Ecomm_web_api.Models.Entity
{
    public enum OrderStatus
    {
        Pending,
        Shipped
    }

    public class Order
    {
        [Key]
        public Guid Id { get; set; }   // Auto-generated PK

        [Required]
        public Guid UserId { get; set; }   // FK to User table
        public User User { get; set; }    // Navigation property

        [Required]
        public int OrderNumber { get; set; }  // Auto-generated

        [Required]
        public int OrderTotal { get; set; }   // Sum of OrderItems

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // 🔗 One Order has many OrderItems
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
