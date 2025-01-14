using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository.MongoDB.Interfaces.MongoDB;
using MongoDB.Driver;

namespace Humanity.Repository.MongoDB.Repositories.MongoDB
{
    public class MongoReceiptRepository : MongoRepository<Receipt>, IMongoReceiptRepository
    {
        public MongoReceiptRepository(IMongoDatabase database) : base(database, "Receipts")
        {
        }
    }
}
