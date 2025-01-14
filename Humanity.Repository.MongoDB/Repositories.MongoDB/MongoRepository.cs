using Humanity.Repository.MongoDB.Interfaces.MongoDB;
using MongoDB.Driver;

namespace Humanity.Repository.MongoDB.Repositories.MongoDB
{
    // Generička klasa koja implementira osnovne CRUD operacije za MongoDB repozitorijum
    public class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            // Inicijalizacija kolekcije koristeći bazu podataka i ime kolekcije
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<T> Add(T entity)
        {
            // Ubacivanje entiteta u kolekciju
            await _collection.InsertOneAsync(entity);
            // Vraćanje ubačenog entiteta
            return entity;
        }

        public async Task Delete(string id)
        {
            // Brisanje entiteta iz kolekcije koristeći filter po ID-u
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));
        }

        public async Task<List<T>> GetAll()
        {
            // Vraćanje svih entiteta kao lista
            return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
        }

        public async Task<T> GetById(string id)
        {
            // Pronalaženje entiteta po ID-u u kolekciji
            return await _collection.Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }

        public async Task<T> Update(T entity)
        {
            // Zamena starog entiteta novim koristeći ID kao filter
            var id = (string)typeof(T).GetProperty("Id").GetValue(entity);
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Id", id), entity);
            // Vraćanje ažuriranog entiteta
            return entity;
        }
    }
}
