using Humanity.Domain.Enums;

namespace Humanity.Application.DTOs
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public bool IsAnonymous { get; set; }
        public string? Image { get; set; }
        public List<DonationCategory>? RegisteredCategories { get; set; }
    }
}
