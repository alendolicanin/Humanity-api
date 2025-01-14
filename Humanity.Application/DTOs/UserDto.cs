using Humanity.Domain.Enums;

namespace Humanity.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public bool IsAnonymous { get; set; }
        public string Email { get; set; }
        public string? Image { get; set; }
        public List<DonationCategory>? RegisteredCategories { get; set; }
    }
}
