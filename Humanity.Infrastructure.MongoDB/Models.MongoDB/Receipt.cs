using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Humanity.Infrastructure.MongoDB.Models.MongoDB
{
    public class Receipt
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // ID dokumenta
        public string DistributedDonationId { get; set; } // Referenca na podeljenu donaciju
        public DateTime DateIssued { get; set; } // Datum izdavanja potvrde
        public decimal Value { get; set; } // Vrednost donacije koja je potvrđena
        public string DonorInfo { get; set; } // Informacije o donatoru
        public string RecipientInfo { get; set; } // Informacije o primaocu
        public string Signature { get; set; } // Potpis korisnika
    }
}
