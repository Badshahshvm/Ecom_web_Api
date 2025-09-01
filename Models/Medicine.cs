using System;
using System.ComponentModel.DataAnnotations;

namespace Ecomm_web_api.Models.Entity
{
    public enum MedicineStatus
    {
        Available,
        OutOfStock,
        Expired,
        Discontinued
    }

    public class Medicine
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string MedicineName { get; set; }

        [Range(0, int.MaxValue)]
        public int UnitPrice { get; set; }

        [Range(0, 100)]
        public int Discount { get; set; } = 0;  // percentage discount

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [MaxLength(100)]
        public string? Disease { get; set; }

        [MaxLength(300)]
        public string? Uses { get; set; }

        [Required]
        public DateOnly ExpDate { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public MedicineStatus Status { get; set; } = MedicineStatus.Available;
    }
}
