namespace Humanity.Application.DTOs
{
    public class ThankYouNoteDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string DonorId { get; set; }
        public string SenderName { get; set; }
        public string DonorName { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
    }
}
