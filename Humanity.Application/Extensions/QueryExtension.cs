using System.Linq.Expressions;

namespace Humanity.API.Extensions
{
    public static class QueryExtension
    {
        // Ekstenzija za primenu sortiranja
        // query - upit nad podacima koji se izvršava sortiranje, queryObj - objekat koji sadrži
        // informacije o sortiranju, sortColumns - mape za sortiranje kolona, povezuje nazive kolona za sortiranje
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, IQueryObj queryObj, Dictionary<string, Expression<Func<T, object>>> sortColumns)
        {
            // Ako nije navedena kolona po kojoj se vrši sortiranje ili ne postoji
            // u mapama za sortiranje, vratiti upit  
            if (string.IsNullOrEmpty(queryObj.SortBy) || !sortColumns.ContainsKey(queryObj.SortBy))
            {
                return query;
            }

            // Ako je navedeno da li je sortiranje uzlazno ili silazno 
            bool isSortAscending = queryObj.IsSortAscending ?? true;

            // Ako je sortiranje uzlazno, koristimo OrderBy, inače koristimo OrderByDescending
            if (isSortAscending)
            {
                // Sortiranje po koloni koja je navedena u queryObj.SortBy
                query = query.OrderBy(sortColumns[queryObj.SortBy]);
            }
            else
            {
                // Sortiranje po koloni koja je navedena u queryObj.SortBy
                query = query.OrderByDescending(sortColumns[queryObj.SortBy]);
            }

            // Vraćanje sortiranog upita
            return query;
        }

        // Ekstenzija za primenu paginacije
        // query - upit koji se izvršava, queryObj - objekat koji sadrži informacije o paginaciji
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObj queryObj)
        {
            // Provera da li su parametri straničenja validni
            // Ako su `Page` ili `PageSize` null ili imaju vrednosti manje ili jednake 0,
            // straničenje se ne primenjuje i vraća se originalni query.
            if (queryObj.Page == null || queryObj.PageSize == null || queryObj.Page <= 0 || queryObj.PageSize <= 0)
            {
                return query; // Ne primenjuj straničenje ako su parametri neispravni
            }

            // Ako su parametri validni, preuzimaju se njihove vrednosti.
            // `Page` predstavlja trenutnu stranicu, dok `PageSize` predstavlja broj stavki po stranici.
            int page = queryObj.Page.Value;
            int pageSize = queryObj.PageSize.Value;

            // Primena straničenja:
            // - Preskačemo `(page - 1) * pageSize` stavki (preskačemo sve prethodne stranice).
            // - Uzimamo `pageSize` stavki koje pripadaju trenutnoj stranici.
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
