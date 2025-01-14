using Humanity.Domain.Models;
using Humanity.Infrastructure;
using Humanity.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Humanity.Repository.Repositories
{
    public class ThankYouNoteRepository : Repository<ThankYouNote, int>, IThankYouNoteRepository
    {
        private readonly PlutoContext _context;
        public ThankYouNoteRepository(PlutoContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<ThankYouNote> GetAll()
        {
            return _context.ThankYouNotes.Include(tn => tn.Sender).Include(tn => tn.Donor).AsQueryable();
        }
    }
}
