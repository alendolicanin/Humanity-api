using Humanity.Repository.Neo4jDB.Interfaces.Neo4jDB;

namespace Humanity.Repository.Neo4jDB
{
    public interface INeo4jUnitOfWork : IDisposable
    {
        INeo4jThankYouNoteRepository ThankYouNoteRepository { get; }
        Task<bool> CompleteAsync();
    }
}
