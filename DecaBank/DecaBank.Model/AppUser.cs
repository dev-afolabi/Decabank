using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DecaBank.Model
{
    public class AppUser : IdentityUser
    {
        [Required]
        [MaxLength(10,ErrorMessage ="Firstname cannot be greater than 10 characters")]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Lastname cannot be greater than 10 characters")]
        public string Lastname { get; set; }
        public Address Address { get; set; }

    }
}
