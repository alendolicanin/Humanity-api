namespace Humanity.API.Middleware
{
    public static class IPWhitelistMiddlewareExtensions
    {
        // Metoda za registraciju IPWhitelistMiddleware-a u servis kolekciju (Registruje potrebne servise za rad middleware-a)
        public static IServiceCollection AddIPWhitelist(this IServiceCollection services, string[] whitelistedIPs)
        {
            // Registruje listu dozvoljenih IP adresa kao singleton servis
            services.AddSingleton(whitelistedIPs);
            // Dodaje podršku za memorijsko keširanje (IMemoryCache)
            services.AddMemoryCache();
            // Vraća modifikovanu servis kolekciju
            return services;
        }

        // Metoda za dodavanje IPWhitelistMiddleware-a u pipeline aplikacije
        // (Middleware se registruje u pipeline-u korišćenjem metode UseIPWhitelist,
        // što osigurava da se svaki zahtev procesuira kroz ovaj middleware)
        public static IApplicationBuilder UseIPWhitelist(this IApplicationBuilder builder)
        {
            // Dodaje IPWhitelistMiddleware u middleware pipeline
            return builder.UseMiddleware<IPWhitelistMiddleware>();
        }
    }
}
