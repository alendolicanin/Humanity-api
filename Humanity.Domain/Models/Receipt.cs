using System.ComponentModel.DataAnnotations;

namespace Humanity.Domain.Models
{
    public class Receipt
    {
        [Key]
        public int Id { get; set; }
        public int DistributedDonationId { get; set; }
        public DistributedDonation DistributedDonation { get; set; } // Referenca na podeljenu donaciju
        public DateTime DateIssued { get; set; } // Datum izdavanja potvrde
        public decimal Value { get; set; } // Vrednost donacije koja je potvrđena
        public string DonorInfo { get; set; } // Informacije o donatoru, popunjeno samo ako donator nije anoniman
        public string RecipientInfo { get; set; } // Informacije o primaocu donacije
        public string Signature { get; set; } // Potpis korisnika kao dokaz isporuke
    }
}
