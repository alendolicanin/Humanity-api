using System.ComponentModel.DataAnnotations;

namespace Humanity.Domain.Models
{
    public class DistributedDonation
    {
        [Key]
        public int Id { get; set; }
        public int DonationId { get; set; }
        public Donation Donation { get; set; } // Referenca na originalnu donaciju
        public string RecipientId { get; set; }
        public User Recipient { get; set; } // Referenca na korisnika koji je primalac donacije
        public DateTime DateDistributed { get; set; } // Datum kada je donacija podeljena
        public decimal Value { get; set; } // Vrednost podeljene donacije
        public Receipt Receipt { get; set; } // Povezana potvrda za ovu donaciju
    }
}
