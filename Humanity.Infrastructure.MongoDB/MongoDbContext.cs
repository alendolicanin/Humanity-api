using Humanity.Infrastructure.MongoDB.Models.MongoDB; 
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Humanity.Infrastructure.MongoDB
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public IMongoClient Client { get; }

        // Konstruktor koji prima opcije MongoDbSettings kao parametar
        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            // Inicijalizacija MongoDB klijenta koristeći connection string iz podešavanja
            Client = new MongoClient(settings.Value.ConnectionString);
            // Dobijanje instance baze podataka koristeći ime baze iz podešavanja
            _database = Client.GetDatabase(settings.Value.DatabaseName);
        }

        // Svojstva koja vraćaju kolekcije iz baze podataka
        public IMongoCollection<Donation> Donations => _database.GetCollection<Donation>("Donations");
        // Kolekcija sadrži dokumente koji odgovaraju klasi Donation         // <Donation> znači da očekujemo da dokumenti u toj kolekciji imaju strukturu definisanu u klasi Donation
        public IMongoCollection<DistributedDonation> DistributedDonations => _database.GetCollection<DistributedDonation>("DistributedDonations");
        public IMongoCollection<Receipt> Receipts => _database.GetCollection<Receipt>("Receipts");
        public IMongoCollection<ThankYouNote> ThankYouNotes => _database.GetCollection<ThankYouNote>("ThankYouNotes");

        // Svojstvo koje vraća instancu baze podataka
        public IMongoDatabase Database => _database;
    }
}
