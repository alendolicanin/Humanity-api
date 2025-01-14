using Humanity.Infrastructure.MongoDB;
using Humanity.Repository.MongoDB.Interfaces.MongoDB;
using Humanity.Repository.MongoDB.Repositories.MongoDB;
using MongoDB.Driver;

namespace Humanity.Repository.MongoDB
{
    // Klasa koja implementira Unit of Work za MongoDB, grupišući različite
    // repozitorijume u jednu transakciju
    public class MongoUnitOfWork : IMongoUnitOfWork
    {
        // Privatno polje za čuvanje konteksta MongoDB-a
        private readonly MongoDbContext _context;
        // Privatno polje za čuvanje sesije MongoDB-a
        private readonly IClientSessionHandle _session;

        public IMongoDonationRepository DonationRepository { get; }
        public IMongoDistributedDonationRepository DistributedDonationRepository { get; }
        public IMongoReceiptRepository ReceiptRepository { get; }
        public IMongoThankYouNoteRepository ThankYouNoteRepository { get; }

        // Konstruktor koji prima kontekst MongoDB-a i inicijalizuje sesiju i repozitorijume
        public MongoUnitOfWork(MongoDbContext context)
        {
            _context = context;
            _session = _context.Client.StartSession();

            // Inicijalizacija repozitorijuma sa instancom baze podataka iz konteksta
            DonationRepository = new MongoDonationRepository(_context.Database);
            DistributedDonationRepository = new MongoDistributedDonationRepository(_context.Database);
            ReceiptRepository = new MongoReceiptRepository(_context.Database);
            ThankYouNoteRepository = new MongoThankYouNoteRepository(_context.Database);
        }

        // Metoda za potvrđivanje i čuvanje svih promena u bazi podataka u okviru transakcije
        public async Task<bool> CompleteAsync()
        {
            try
            {
                // Pokreni transakciju
                _session.StartTransaction();

                // Potvrdi transakciju
                await _session.CommitTransactionAsync();
                return true;
            }
            catch
            {
                // Poništi transakciju u slučaju greške
                await _session.AbortTransactionAsync();
                return false;
            }
        }

        // Implementacija IDisposable interfejsa za pravilno oslobađanje resursa
        public void Dispose()
        {
            // Oslobađanje sesije ako nije null
            _session?.Dispose();
        }
    }
}
