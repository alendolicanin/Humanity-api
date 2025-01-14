using Humanity.Domain.Enums;

namespace Humanity.Application.DTOs
{
    public class UpdateDonationDto
    {
        public decimal Value { get; set; }
        public DonationCategory Category { get; set; }
    }
}
