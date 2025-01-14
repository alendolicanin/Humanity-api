using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Humanity.Infrastructure.MongoDB.Models.MongoDB
{
    public class DistributedDonation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // ID dokumenta
        public string DonationId { get; set; } // ID originalne donacije
        public string RecipientId { get; set; } // ID primaoca donacije
        public DateTime DateDistributed { get; set; } // Datum kada je donacija podeljena
        public decimal Value { get; set; } // Vrednost podeljene donacije
    }
}
