using Humanity.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Humanity.Application.DTOs
{
    public class RegisterDto
    {
        [Required, MinLength(3), MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        public string LastName { get; set; }

        [Required, MinLength(2)]
        public string City { get; set; }

        [Required, Range(18, 65)]
        public int Age { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public bool IsAnonymous { get; set; }

        [Required, MinLength(3), MaxLength(100)]
        public string Email { get; set; }

        [Required, MinLength(8), MaxLength(16)]
        public string Password { get; set; }
    }
}
