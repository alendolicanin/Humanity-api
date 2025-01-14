using Humanity.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Humanity.Domain.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }
        public string DonorId { get; set; }
        public User Donor { get; set; } // Referenca na korisnika koji je donator
        public DateTime DateReceived { get; set; } // Datum kada je donacija primljena
        public decimal Value { get; set; } // Vrednost donacije
        public DonationCategory Category { get; set; }
        public bool IsAnonymous { get; set; } // Da li je donacija anonimna
        public bool IsDistributed { get; set; } // Da li je donacija podeljena
        public List<DistributedDonation> DistributedDonations { get; set; } // Lista podeljenih donacija povezanih sa ovom donacijom
    }
}
