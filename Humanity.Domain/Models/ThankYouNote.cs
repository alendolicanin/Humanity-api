using System.ComponentModel.DataAnnotations;

namespace Humanity.Domain.Models
{
    public class ThankYouNote
    {
        [Key]
        public int Id { get; set; }
        public string SenderId { get; set; }
        public User Sender { get; set; } // Referenca na korisnika koji je poslao zahvalnicu
        public string DonorId { get; set; }
        public User Donor { get; set; } // Referenca na korisnika koji je donator
        public string Message { get; set; } // Poruka zahvalnice
        public int Rating { get; set; } // Ocena donacije (od 1 do 10)
    }
}
