using Humanity.Domain.Models;

namespace Humanity.Repository.Interfaces
{
    // Interfejs za repozitorijum DistributedDonation entiteta, koji
    // nasleđuje generički IRepository interfejs
    public interface IDistributedDonationRepository : IRepository<DistributedDonation, int>
    {
        IQueryable<DistributedDonation> GetAll();
    }
}
