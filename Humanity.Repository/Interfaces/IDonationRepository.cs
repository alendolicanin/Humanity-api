using Humanity.Domain.Models;

namespace Humanity.Repository.Interfaces
{
    public interface IDonationRepository : IRepository<Donation, int>
    {
        //  IQueryable omogućava odloženo izvršavanje upita
        //  što znači da se upit ne izvršava odmah
        //  kada se definiše, već tek kada se enumerira
        //  (npr. kada se pozove ToList() ili FirstOrDefault() )

        // IQueryable<Donation> GetAll() omogućava dalju manipulaciju
        // upitom pre nego što se on izvrši

        IQueryable<Donation> GetAll(); // Omogućava efikasno filtriranje, sortiranje i paginaciju pre nego što se podaci preuzmu iz baze
    }
}
