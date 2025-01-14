using Humanity.Domain.Enums;

namespace Humanity.Application.DTOs
{
    public class DonationDto
    {
        public int Id { get; set; }
        public string DonorId { get; set; }
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public DateTime DateReceived { get; set; }
        public decimal Value { get; set; }
        public DonationCategory Category { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsDistributed { get; set; }
    }
}
