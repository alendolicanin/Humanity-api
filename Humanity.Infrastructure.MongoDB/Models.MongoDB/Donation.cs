using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Humanity.Infrastructure.MongoDB.Models.MongoDB
{
    public class Donation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // ID dokumenta
        public string DonorId { get; set; } // ID korisnika donatora
        public DateTime DateReceived { get; set; } // Datum kada je donacija primljena
        public decimal Value { get; set; } // Vrednost donacije
        public DonationCategory Category { get; set; } // Kategorija donacije
        public bool IsAnonymous { get; set; } // Da li je donacija anonimna
        public bool IsDistributed { get; set; } // Da li je donacija podeljena
    }
    public enum DonationCategory
    {
        Food, // Hrana 0
        Clothing, // Odeća 1
        Footwear, // Obuća 2
        Money // Novac 3
    }
}
