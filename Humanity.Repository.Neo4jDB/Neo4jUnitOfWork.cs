using Humanity.Repository.Neo4jDB.Interfaces.Neo4jDB;
using Humanity.Repository.Neo4jDB.Repositories.Neo4jDB;
using Neo4j.Driver;

namespace Humanity.Repository.Neo4jDB
{
    // Klasa koja implementira Unit of Work za Neo4j, grupišući različite repozitorijume u
    // jednu transakciju
    public class Neo4jUnitOfWork : INeo4jUnitOfWork
    {
        // Privatno polje za čuvanje sesije Neo4j-a
        private readonly IAsyncSession _session;
        // Privatno polje za čuvanje transakcije Neo4j-a
        private IAsyncTransaction _transaction;

        public INeo4jThankYouNoteRepository ThankYouNoteRepository { get; private set; }

        // Konstruktor koji prima instancu IAsyncSession i inicijalizuje transakciju
        public Neo4jUnitOfWork(IAsyncSession session)
        {
            _session = session;
            // Inicijalizujemo transakciju; BeginTransactionAsync pokreće novu transakciju
            _transaction = _session.BeginTransactionAsync().GetAwaiter().GetResult();

            ThankYouNoteRepository = new Neo4jThankYouNoteRepository(_transaction);
        }

        // Metoda za potvrđivanje i čuvanje svih promena u bazi podataka u okviru transakcije
        public async Task<bool> CompleteAsync()
        {
            try
            {
                await _transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _transaction.RollbackAsync();
                return false;
            }
            finally
            {
                // Bez obzira na ishod, oslobađamo trenutnu transakciju
                await _transaction.DisposeAsync();
                // Otvaramo novu transakciju za buduće operacije
                _transaction = await _session.BeginTransactionAsync();
            }
        }

        // Implementacija IDisposable interfejsa za pravilno oslobađanje resursa
        public void Dispose()
        {
            // Bezbedno oslobađamo transakciju ako nije null
            _transaction?.DisposeAsync().GetAwaiter().GetResult();
            // Bezbedno oslobađamo sesiju ako nije null
            _session.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
