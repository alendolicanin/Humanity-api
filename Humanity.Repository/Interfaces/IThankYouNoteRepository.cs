using Humanity.Domain.Models;

namespace Humanity.Repository.Interfaces
{
    public interface IThankYouNoteRepository : IRepository<ThankYouNote, int>
    {
        IQueryable<ThankYouNote> GetAll();
    }
}
