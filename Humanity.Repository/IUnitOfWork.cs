using Humanity.Repository.Interfaces;

namespace Humanity.Repository
{
    // Interfejs za Unit of Work koja grupiše različite repozitorijume i
    // pruža transakcione operacije
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IDonationRepository DonationRepository { get; }
        IDistributedDonationRepository DistributedDonationRepository { get; }
        IReceiptRepository ReceiptRepository { get; }
        IThankYouNoteRepository ThankYouNoteRepository { get; }

        // Metoda za potvrđivanje i čuvanje svih promena u bazi podataka
        Task<bool> CompleteAsync();
    }
}
