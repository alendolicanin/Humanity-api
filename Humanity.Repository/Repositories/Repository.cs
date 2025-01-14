using Humanity.Infrastructure;
using Humanity.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Humanity.Repository.Repositories
{
    // Generička klasa koja implementira osnovne CRUD operacije za repozitorijum
    public class Repository<T, TKey> : IRepository<T, TKey> where T : class
    {
        private readonly PlutoContext _context;

        public Repository(PlutoContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            // Dodavanje entiteta u odgovarajući DbSet
            await _context.Set<T>().AddAsync(entity);
            // Vraćanje dodatog entiteta
            return entity;
        }

        public async Task Delete(T entity)
        {
            // Uklanjanje entiteta iz odgovarajućeg DbSet-a
            _context.Set<T>().Remove(entity);
            // Čuvanje promena u bazi podataka
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAll()
        {
            // Vraćanje svih entiteta kao lista
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(TKey id)
        {
            // Pronalaženje entiteta po ID-u
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Update(T entity)
        {
            // Ažuriranje entiteta u odgovarajućem DbSet-u
            _context.Set<T>().Update(entity);
            // Vraćanje ažuriranog entiteta
            return entity;
        }
    }
}
