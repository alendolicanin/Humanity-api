namespace Humanity.Infrastructure.Neo4jDB.Models.Neo4jDB
{
    public class ThankYouNote
    {
        public string Id { get; set; } // ID dokumenta
        public string SenderId { get; set; } // ID korisnika koji šalje zahvalnicu
        public string DonorId { get; set; } // ID korisnika donatora
        public string Message { get; set; } // Poruka zahvalnice
        public int Rating { get; set; } // Ocena donacije (od 1 do 10)
    }
}
