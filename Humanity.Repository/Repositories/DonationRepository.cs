using Humanity.Domain.Models;
using Humanity.Infrastructure;
using Humanity.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Humanity.Repository.Repositories
{
    // Klasa koja implementira repozitorijum za donacije, nasleđuje
    // generički repozitorijum i implementira specifičan interfejs za donacije
    public class DonationRepository : Repository<Donation, int>, IDonationRepository
    {
        private readonly PlutoContext _context;

        // Konstruktor koji prima kontekst baze podataka i
        // prosleđuje ga osnovnoj klasi (Repository)
        public DonationRepository(PlutoContext context) : base(context)
        {
            _context = context;
        }

        // Implementacija metode GetAll iz IDonationRepository interfejsa
        public IQueryable<Donation> GetAll()
        {
            // Vraćanje svih donacija kao IQueryable za omogućavanje
            // fleksibilnih upita
            return _context.Donations.Include(d => d.Donor).AsQueryable();
        }
    }
}
