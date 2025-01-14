using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Humanity.Infrastructure.MongoDB.Models.MongoDB
{
    public class ThankYouNote
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // ID dokumenta
        public string SenderId { get; set; } // ID korisnika koji šalje zahvalnicu
        public string DonorId { get; set; } // ID korisnika donatora
        public string Message { get; set; } // Poruka zahvalnice
        public int Rating { get; set; } // Ocena donacije (od 1 do 10)
    }
}
