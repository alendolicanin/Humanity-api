using Humanity.Domain.Models;
using Humanity.Infrastructure;
using Humanity.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Humanity.Repository.Repositories
{
    // Klasa koja implementira repozitorijum za distribuisane donacije,
    // nasleđuje generički repozitorijum i implementira specifičan interfejs
    // za distribuisane donacije
    public class DistributedDonationRepository : Repository<DistributedDonation, int>, IDistributedDonationRepository
    {
        private readonly PlutoContext _context;
        // Konstruktor koji prima kontekst baze podataka i prosleđuje
        // ga osnovnoj klasi (Repository)
        public DistributedDonationRepository(PlutoContext context) : base(context)
        {
            _context = context;
        }
        // Implementacija metode GetAll iz IDistributedDonationRepository interfejsa
        public IQueryable<DistributedDonation> GetAll()
        {
            // Vraćanje svih donacija kao IQueryable za omogućavanje fleksibilnih upita
            // Uključujemo entitete Recipient i Donation da bismo pristupili njihovim podacima
            return _context.DistributedDonations
                .Include(dd => dd.Recipient) // Za pristup podacima primaoca
                .Include(dd => dd.Donation)  // Za pristup podacima o donaciji
                .Include(dd => dd.Receipt)   // Za pristup podacima o računu
                .AsQueryable();
        }
    }
}
