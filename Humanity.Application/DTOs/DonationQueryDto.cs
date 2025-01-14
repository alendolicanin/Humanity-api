using Humanity.API.Extensions;

namespace Humanity.Application.DTOs
{
    public class DonationQueryDto : IQueryObj
    {
        // Naziv kolone po kojoj se vrši sortiranje (npr. "DateReceived" ili "Value")
        public string? SortBy { get; set; }

        // Da li je sortiranje rastuće (true) ili opadajuće (false)
        public bool? IsSortAscending { get; set; }
        
        // Broj stranice za straničenje
        public int? Page { get; set; }
        
        // Broj stavki po stranici za straničenje
        public int? PageSize { get; set; }
    
        // Filtriranje po datumu primanja donacije
        public DateTime? DateReceived { get; set; }

        // Novo: Filtriranje po kategoriji
        public int? Category { get; set; }

        // Novo: Filtriranje po imenu/prezimenu donatora
        public string? DonorName { get; set; }
    }
}
