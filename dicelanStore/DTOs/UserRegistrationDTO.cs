using System.ComponentModel.DataAnnotations;

namespace dicelanStore.DTOs
{
    public class UserRegistrationDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string? PhoneNumber { get; set; } = string.Empty;

        public string? Email { get; set; }

        public byte[]? Avatar { get; set; }

        public string? Street { get; set; } // Street can be optional
        public string? City { get; set; } // City can be optional
        public string? Province { get; set; } // Province can be optional
        public string? Ward { get; set; } // Ward can be optional
        public string? ZipCode { get; set; } // Zip code can be optional
    }
}
