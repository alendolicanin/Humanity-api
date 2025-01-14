using Microsoft.Extensions.Options;
using Neo4j.Driver;

namespace Humanity.Infrastructure.Neo4jDB
{
    public class Neo4jContext : IDisposable
    {
        private readonly IDriver _driver;

        // Konstruktor koji prima opcije Neo4jSettings kao parametar
        public Neo4jContext(IOptions<Neo4jSettings> settings)
        {
            // Inicijalizacija Neo4j drajvera koristeći URI i autentifikacione podatke iz podešavanja
            _driver = GraphDatabase.Driver(settings.Value.Uri, AuthTokens.Basic(settings.Value.Username, settings.Value.Password));
        }

        // Metoda za dobijanje asinhrone sesije sa podrazumevanim načinom pristupa kao Write
        public IAsyncSession GetSession()
        {
            // Vraćanje asinhrone sesije sa podrazumevanim načinom pristupa kao Write
            return _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write));
        }

        // Implementacija IDisposable interfejsa za pravilno oslobađanje resursa
        public void Dispose()
        {
            // Oslobađanje drajvera ako nije null
            _driver?.Dispose();
        }
    }
}
