using Humanity.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Humanity.Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public UserRole Role { get; set; } // Uloga korisnika
        public bool IsActive { get; set; } // Status da li je korisnik aktivan
        public bool IsAnonymous { get; set; } // Da li je donator anoniman
        public string? Image { get; set; } // Slika korisnika
        public List<Donation> Donations { get; set; } // Lista donacija koje je korisnik dao
        public List<DistributedDonation> ReceivedDonations { get; set; } // Lista donacija koje je korisnik primio
        public List<ThankYouNote> SentThankYouNotes { get; set; } // Lista zahvalnica koje je korisnik poslao
        public List<ThankYouNote> ReceivedThankYouNotes { get; set; } // Lista zahvalnica koje je korisnik primio
        public List<DonationCategory>? RegisteredCategories { get; set; } // Lista koja čuva kategorije za koje se korisnik prijavio
    }
}
