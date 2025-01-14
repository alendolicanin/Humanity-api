namespace Humanity.Application.DTOs
{
    public class ReceiptDto
    {
        public int Id { get; set; }
        public int DistributedDonationId { get; set; }
        public DateTime DateIssued { get; set; }
        public decimal Value { get; set; }
        public string DonorInfo { get; set; }
        public string RecipientInfo { get; set; }
        public string Signature { get; set; }
    }
}
