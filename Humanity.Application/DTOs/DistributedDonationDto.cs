namespace Humanity.Application.DTOs
{
    public class DistributedDonationDto
    {
        public int Id { get; set; }
        public int DonationId { get; set; }
        public string DonorId { get; set; }
        public string RecipientId { get; set; }
        public string RecipientFirstName { get; set; }
        public string RecipientLastName { get; set; }
        public DateTime DateDistributed { get; set; }
        public decimal Value { get; set; }
        public string Category { get; set; }
        public ReceiptDto Receipt { get; set; }
    }
}
