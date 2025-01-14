using Humanity.Repository.Neo4jDB.Interfaces.Neo4jDB;
using Humanity.Infrastructure.Neo4jDB.Models.Neo4jDB;
using Neo4j.Driver;

namespace Humanity.Repository.Neo4jDB.Repositories.Neo4jDB
{
    public class Neo4jThankYouNoteRepository : Neo4jRepository<ThankYouNote>, INeo4jThankYouNoteRepository
    {
        public Neo4jThankYouNoteRepository(IAsyncTransaction transaction) : base(transaction)
        {
        }
    }
}
