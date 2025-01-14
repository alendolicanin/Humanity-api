namespace Humanity.Application.DTOs
{
    public class CreateDistributedDonationDto
    {
        public int DonationId { get; set; }
        public string RecipientId { get; set; }
        public DateTime DateDistributed { get; set; }
        public decimal Value { get; set; }
    }
}
