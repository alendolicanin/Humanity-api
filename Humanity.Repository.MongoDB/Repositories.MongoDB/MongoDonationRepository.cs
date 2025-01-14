using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository.MongoDB.Interfaces.MongoDB;
using MongoDB.Driver;

namespace Humanity.Repository.MongoDB.Repositories.MongoDB
{
    public class MongoDonationRepository : MongoRepository<Donation>, IMongoDonationRepository
    {
        public MongoDonationRepository(IMongoDatabase database) : base(database, "Donations")
        {
        }
    }
}
