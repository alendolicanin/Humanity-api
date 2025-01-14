namespace Humanity.API.Extensions
{
    public interface IQueryObj
    {
        // Sortiranje
        public string? SortBy { get; set; } // Naziv kolone po kojoj se vrši sortiranje
        public bool? IsSortAscending { get; set; } // Da li je sortiranje rastuće (true) ili opadajuće (false)

        // Paginacija
        public int? Page { get; set; } // Broj stranice
        public int? PageSize { get; set; } // Broj stavki po stranici
    }
}
