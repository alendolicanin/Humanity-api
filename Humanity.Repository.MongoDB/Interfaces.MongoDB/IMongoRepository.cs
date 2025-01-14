namespace Humanity.Repository.MongoDB.Interfaces.MongoDB
{
    public interface IMongoRepository<T> where T : class
    {
        Task<T> GetById(string id);
        Task<List<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(string id);
    }
}
