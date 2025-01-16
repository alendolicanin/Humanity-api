using Humanity.Repository.Neo4jDB.Interfaces.Neo4jDB;
using Neo4j.Driver;

namespace Humanity.Repository.Neo4jDB.Repositories.Neo4jDB
{
    // Generička klasa koja implementira osnovne CRUD operacije za Neo4j repozitorijum
    public class Neo4jRepository<T> : INeo4jRepository<T> where T : class, new()
    {
        // Polje za transakciju koja se koristi za izvršavanje upita u Neo4j bazi podataka
        private readonly IAsyncTransaction _transaction;

        // Konstruktor koji inicijalizuje transakciju
        public Neo4jRepository(IAsyncTransaction transaction)
        {
            _transaction = transaction; // Postavlja prosleđenu transakciju u privatno polje
        }

        // Metoda za preuzimanje jednog čvora iz baze na osnovu ID-a
        public async Task<T> GetById(string id)
        {
            // Cypher upit za pronalaženje čvora sa datim ID-om
            var query = $"MATCH (n:{typeof(T).Name}) WHERE n.Id = $id RETURN n";

            // Izvršavanje upita sa parametrom `id`
            var result = await _transaction.RunAsync(query, new { id });

            // Koristimo ToListAsync i uzimamo prvi rezultat
            var records = await result.ToListAsync();
            var record = records.FirstOrDefault();

            // Ako nema rezultata, vraćamo null
            if (record == null) return null;

            // Mapiranje čvora na entitet
            return MapNodeToEntity(record["n"].As<INode>());
        }

        // Metoda za preuzimanje svih čvorova određenog tipa
        public async Task<List<T>> GetAll()
        {
            // Cypher upit za pronalaženje svih čvorova određenog tipa
            var query = $"MATCH (n:{typeof(T).Name}) RETURN n";

            // Izvršavanje upita
            var result = await _transaction.RunAsync(query);

            // Iteriranje kroz sve rezultate i mapiranje na entitete
            var entities = new List<T>();
            await foreach (var record in result)
            {
                entities.Add(MapNodeToEntity(record["n"].As<INode>()));
            }

            return entities;
        }

        // Metoda za dodavanje novog čvora u bazu podataka
        public async Task<T> Add(T entity)
        {
            // Generisanje ID-a ako nije postavljen
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null && idProperty.GetValue(entity) == null)
            {
                idProperty.SetValue(entity, Guid.NewGuid().ToString());
            }

            // Cypher upit za kreiranje novog čvora sa svim svojstvima
            var query = $"CREATE (n:{typeof(T).Name} $props) RETURN n";

            // Izvršavanje upita sa entitetom kao parametrima
            var result = await _transaction.RunAsync(query, new { props = entity });

            // Preuzimanje rezultata (novokreiranog čvora)
            var record = await result.SingleAsync();

            // Mapiranje čvora na entitet
            return MapNodeToEntity(record["n"].As<INode>());
        }

        // Metoda za ažuriranje postojećeg čvora u bazi podataka
        public async Task<T> Update(T entity)
        {
            // Cypher upit za pronalaženje čvora po ID-u i ažuriranje njegovih svojstava
            var query = $"MATCH (n:{typeof(T).Name}) WHERE n.Id = $id SET n += $props RETURN n";

            // Ekstrakcija vrednosti `Id` iz entiteta pomoću refleksije
            var id = typeof(T).GetProperty("Id")?.GetValue(entity)?.ToString();

            // Izvršavanje upita sa ID-om i svojstvima entiteta kao parametrima
            var result = await _transaction.RunAsync(query, new { id, props = entity });

            // Preuzimanje rezultata (ažuriranog čvora)
            var record = await result.SingleAsync();

            // Mapiranje čvora na entitet
            return MapNodeToEntity(record["n"].As<INode>());
        }

        // Metoda za brisanje čvora iz baze podataka na osnovu ID-a
        public async Task Delete(string id)
        {
            // Cypher upit za brisanje čvora sa datim ID-om
            var query = $"MATCH (n:{typeof(T).Name}) WHERE n.Id = $id DELETE n";

            // Izvršavanje upita sa ID-om kao parametrom
            await _transaction.RunAsync(query, new { id });
        }

        // Pomoćna metoda za mapiranje Node na entitet
        private T MapNodeToEntity(INode node)
        {
            // Kreiramo novu instancu generičkog tipa T (entiteta)
            var entity = new T();

            // Prolazimo kroz sva svojstva klase T (entiteta)
            foreach (var property in typeof(T).GetProperties())
            {
                // Proveravamo da li čvor (Node) ima svojstvo sa imenom koje odgovara trenutnom svojstvu entiteta
                // i da li to svojstvo nije null
                if (node.Properties.ContainsKey(property.Name) && node.Properties[property.Name] != null)
                {
                    // Postavljamo vrednost svojstva entiteta koristeći vrednost iz čvora (Node)
                    // Convert.ChangeType se koristi za konverziju tipa vrednosti iz Node-a u odgovarajući tip svojstva entiteta
                    property.SetValue(entity, Convert.ChangeType(node.Properties[property.Name], property.PropertyType));
                }
                // Ukratko: Prolazi kroz sva svojstva entiteta i popunjava ih vrednostima iz svojstava
                // čvora koja imaju odgovarajuće ime
            }
            // Vraćamo popunjenu instancu entiteta
            return entity;
        }
    }
}
