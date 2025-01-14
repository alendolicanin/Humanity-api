using Humanity.Domain.Enums;

namespace Humanity.Application.DTOs
{
    public class CreateDonationDto
    {
        public string DonorId { get; set; }
        public decimal Value { get; set; }
        public DonationCategory Category { get; set; }
    }
}
