using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace Humanity.API.Middleware
{
    // One se automatski koriste kada je IPWhitelistMiddleware registrovan i postavljen u middleware
    // pipeline aplikacije
    public class IPWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IPWhitelistMiddleware> _logger;
        private readonly IMemoryCache _cache;
        private readonly string[] _whitelistedIPs;

        // RequestDelegate next - Delegat koji predstavlja sledeći middleware u pipeline-u
        // ILogger<IPWhitelistMiddleware> logger - Logger za beleženje informacija i grešaka
        // IMemoryCache cache - Cache za čuvanje IP adresa koje su već proverene
        // string[] whitelistedIPs - Niz IP adresa koje su dozvoljene
        public IPWhitelistMiddleware(RequestDelegate next, ILogger<IPWhitelistMiddleware> logger, IMemoryCache cache, string[] whitelistedIPs)
        {
            _next = next;
            _logger = logger;
            _cache = cache;
            _whitelistedIPs = whitelistedIPs;
        }

        // Metoda koja se poziva za svaki HTTP zahtev
        public async Task InvokeAsync(HttpContext context)
        {
            // Dobijanje IP adrese klijenta
            var ipAddress = context.Connection.RemoteIpAddress;

            // Provera da li je IP adresa već u cache-u
            if (!_cache.TryGetValue(ipAddress, out _)) // Middleware koristi IMemoryCache za keširanje već proveravanih IP adresa
            {
                // Logovanje pokušaja pristupa (Middleware loguje pokušaj pristupa za svaku IP adresu koja nije u kešu)
                _logger.LogInformation($"IP address {ipAddress} attempted to access the system at {DateTime.UtcNow}");

                // Provera da li je IP adresa dozvoljena
                if (!IsWhitelisted(ipAddress))
                {
                    // Ako nije, postavljanje statusnog koda 403 (Forbidden) i slanje odgovora
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsync("Forbidden: Your IP address is not whitelisted.");
                    return;
                }

                // Ako je IP adresa dozvoljena, dodavanje u cache sa rokom trajanja od 5 minuta
                _cache.Set(ipAddress, true, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }

            // Pozivanje sledećeg middleware-a u pipeline-u
            await _next(context);
        }

        // Provera da li je IP adresa na listi dozvoljenih
        private bool IsWhitelisted(IPAddress ipAddress)
        {
            // Iteriranje kroz listu dozvoljenih IP adresa
            foreach (var whitelistedIP in _whitelistedIPs)
            {
                // Pokušaj parsiranja IP adrese
                if (IPAddress.TryParse(whitelistedIP, out var whitelistIP))
                {
                    // Provera da li se parsirana IP adresa poklapa sa IP adresom klijenta
                    if (whitelistIP.Equals(ipAddress))
                    {
                        return true;
                    }
                }
            }
            // Ako nijedna IP adresa ne odgovara, vraća se false
            return false;
        }
    }
    // Pipeline je "cevovod" kroz koji prolaze HTTP zahtevi i odgovori. U ovom kodu, middleware
    // IPWhitelistMiddleware je deo tog pipeline-a i služi za kontrolu pristupa na osnovu IP adresa.
    // Svaki zahtev mora proći kroz ovaj middleware pre nego što stigne do ostatka aplikacije.
}
