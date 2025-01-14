using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository.MongoDB.Interfaces.MongoDB;
using MongoDB.Driver;

namespace Humanity.Repository.MongoDB.Repositories.MongoDB
{
    public class MongoDistributedDonationRepository : MongoRepository<DistributedDonation>, IMongoDistributedDonationRepository
    {
        // Konstruktor koji prima instancu MongoDB baze podataka i prosleđuje ime
        // kolekcije osnovnoj klasi
        public MongoDistributedDonationRepository(IMongoDatabase database) : base(database, "DistributedDonations")
        {
        }
    }
}
