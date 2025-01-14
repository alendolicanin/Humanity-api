using Humanity.Repository.MongoDB.Interfaces.MongoDB;

namespace Humanity.Repository.MongoDB
{
    public interface IMongoUnitOfWork : IDisposable
    {
        IMongoDonationRepository DonationRepository { get; }
        IMongoDistributedDonationRepository DistributedDonationRepository { get; }
        IMongoReceiptRepository ReceiptRepository { get; }
        IMongoThankYouNoteRepository ThankYouNoteRepository { get; }
        Task<bool> CompleteAsync();
    }
}
