namespace Humanity.API.Middleware
{
    public static class IPWhitelistMiddlewareExtensions
    {
        // Metoda za registraciju IPWhitelistMiddleware-a u servis kolekciju
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
        public static IApplicationBuilder UseIPWhitelist(this IApplicationBuilder builder)
        {
            // Dodaje IPWhitelistMiddleware u middleware pipeline
            return builder.UseMiddleware<IPWhitelistMiddleware>();
        }
    }
}
