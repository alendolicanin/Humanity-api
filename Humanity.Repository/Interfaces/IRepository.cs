namespace Humanity.Repository.Interfaces
{
    // Generički interfejs za repozitorijum sa osnovnim CRUD operacijama
    public interface IRepository<T, TKey> where T : class
    {
        // Metoda za dobavljanje entiteta po ID-ju
        Task<T> GetById(TKey id);

        // Metoda za dobavljanje svih entiteta
        Task<List<T>> GetAll();
        
        // Metoda za dodavanje novog entiteta
        Task<T> Add(T entity);
        
        // Metoda za ažuriranje entiteta
        Task<T> Update(T entity);

        // Metoda za brisanje entiteta
        Task Delete(T entity);
    }
}
