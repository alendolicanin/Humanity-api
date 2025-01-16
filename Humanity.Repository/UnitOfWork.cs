using Humanity.Infrastructure;
using Humanity.Repository.Interfaces;

namespace Humanity.Repository
{
    // Klasa koja implementira Unit of Work i omogućava grupisanje višestrukih
    // repozitorijuma u jednu transakciju
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlutoContext _context;
        public IUserRepository UserRepository { get; }
        public IDonationRepository DonationRepository { get; }
        public IDistributedDonationRepository DistributedDonationRepository { get; }
        public IReceiptRepository ReceiptRepository { get; }
        public IThankYouNoteRepository ThankYouNoteRepository { get; }

        public UnitOfWork(PlutoContext context,
                          IUserRepository userRepository,
                          IDonationRepository donationRepository,
                          IDistributedDonationRepository distributedDonationRepository,
                          IReceiptRepository receiptRepository,
                          IThankYouNoteRepository thankYouNoteRepository)
        {
            _context = context;
            UserRepository = userRepository;
            DonationRepository = donationRepository;
            DistributedDonationRepository = distributedDonationRepository;
            ReceiptRepository = receiptRepository;
            ThankYouNoteRepository = thankYouNoteRepository;
        }

        // Metoda za potvrđivanje i čuvanje svih promena u bazi podataka
        // kako bi se omogućilo grupisanje i upravljanje skupom operacija kao jedinstvenom transakcijom
        // Njegova osnovna svrha je da obezbedi konsistentnost podataka i pojednostavi rad sa
        // repozitorijumima
        public async Task<bool> CompleteAsync()
        {
            // Čuvanje promena u bazi podataka i vraćanje broja promenjenih redova
            var result = await _context.SaveChangesAsync();
            // Vraćanje true ako je bilo promenjenih redova, u suprotnom false
            return result > 0;
        }
    }
}
