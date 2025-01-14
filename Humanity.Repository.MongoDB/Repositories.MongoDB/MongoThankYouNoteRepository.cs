using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository.MongoDB.Interfaces.MongoDB;
using MongoDB.Driver;

namespace Humanity.Repository.MongoDB.Repositories.MongoDB
{
    public class MongoThankYouNoteRepository : MongoRepository<ThankYouNote>, IMongoThankYouNoteRepository
    {
        public MongoThankYouNoteRepository(IMongoDatabase database) : base(database, "ThankYouNotes")
        {
        }
    }
}
