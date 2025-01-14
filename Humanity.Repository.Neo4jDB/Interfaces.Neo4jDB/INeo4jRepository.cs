namespace Humanity.Repository.Neo4jDB.Interfaces.Neo4jDB
{
    public interface INeo4jRepository<T> where T : class
    {
        Task<T> GetById(string id);
        Task<List<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(string id);
    }
}
