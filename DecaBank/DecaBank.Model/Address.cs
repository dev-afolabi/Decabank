using System.ComponentModel.DataAnnotations;

namespace DecaBank.Model
{
    public class Address : BaseEntity
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Street cannot be greater than 50 characters")]
        public string Street { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "City cannot be greater than 50 characters")]
        public string City { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "State cannot be greater than 50 characters")]
        public string State { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Country cannot be greater than 50 characters")]
        public string Country { get; set; }
        public string AppUserId { get; set; }
    }
}
