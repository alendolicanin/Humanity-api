namespace Humanity.Application.DTOs
{
    public class CreateThankYouNoteDto
    {
        public string SenderId { get; set; }
        public string DonorId { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
    }
}
