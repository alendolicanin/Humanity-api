using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository.MongoDB;

namespace Humanity.Application.Services_MongoDB
{
    public class ThankYouNoteService
    {
        // Privatno polje za upravljanje transakcijama i repozitorijumima
        private readonly IMongoUnitOfWork _unitOfWork;

        public ThankYouNoteService(IMongoUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; // Povezivanje sa UnitOfWork-om
        }

        // Metoda za preuzimanje zahvalnice na osnovu ID-a
        public async Task<MongoDB_ThankYouNoteDto> GetThankYouNoteByIdAsync(string id)
        {
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            // Mapiranje entiteta na DTO objekat i vraćanje korisniku
            return new MongoDB_ThankYouNoteDto
            {
                Id = thankYouNote.Id,
                SenderId = thankYouNote.SenderId,
                DonorId = thankYouNote.DonorId,
                Message = thankYouNote.Message,
                Rating = thankYouNote.Rating
            };
        }

        // Metoda za preuzimanje svih zahvalnica
        public async Task<IEnumerable<MongoDB_ThankYouNoteDto>> GetAllThankYouNotesAsync()
        {
            var thankYouNotes = await _unitOfWork.ThankYouNoteRepository.GetAll();
            // Mapiranje liste entiteta na listu DTO objekata
            return thankYouNotes.Select(thankYouNote => new MongoDB_ThankYouNoteDto
            {
                Id = thankYouNote.Id,
                SenderId = thankYouNote.SenderId,
                DonorId = thankYouNote.DonorId,
                Message = thankYouNote.Message,
                Rating = thankYouNote.Rating
            });
        }

        // Metoda za kreiranje nove zahvalnice
        public async Task<ThankYouNote> CreateThankYouNoteAsync(MongoDB_CreateThankYouNoteDto createThankYouNoteDto)
        {
            // Kreiranje novog entiteta na osnovu DTO objekta
            var thankYouNote = new ThankYouNote
            {
                SenderId = createThankYouNoteDto.SenderId,
                DonorId = createThankYouNoteDto.DonorId,
                Message = createThankYouNoteDto.Message,
                Rating = createThankYouNoteDto.Rating
            };

            // Dodavanje zahvalnice u repozitorijum
            await _unitOfWork.ThankYouNoteRepository.Add(thankYouNote);

            // Čuvanje promena u bazi podataka
            await _unitOfWork.CompleteAsync();

            return thankYouNote; // Vraćanje kreirane zahvalnice
        }

        // Metoda za ažuriranje postojeće zahvalnice
        public async Task<bool> UpdateThankYouNoteAsync(string id, MongoDB_UpdateThankYouNoteDto updateThankYouNoteDto)
        {
            // Dohvatanje postojeće zahvalnice na osnovu ID-a
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null) return false;

            // Ažuriranje polja zahvalnice na osnovu DTO-a
            thankYouNote.Message = updateThankYouNoteDto.Message;
            thankYouNote.Rating = updateThankYouNoteDto.Rating;

            // Ažuriranje entiteta u repozitorijumu
            await _unitOfWork.ThankYouNoteRepository.Update(thankYouNote);

            // Čuvanje promena u bazi podataka
            await _unitOfWork.CompleteAsync();

            return true; // Uspešno ažuriranje
        }

        // Metoda za brisanje zahvalnice
        public async Task<bool> DeleteThankYouNoteAsync(string id)
        {
            // Dohvatanje zahvalnice na osnovu ID-a
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null) return false;

            // Brisanje zahvalnice iz repozitorijuma
            await _unitOfWork.ThankYouNoteRepository.Delete(id);

            // Čuvanje promena u bazi podataka
            await _unitOfWork.CompleteAsync();

            return true; // Uspešno brisanje
        }
    }
    public class MongoDB_CreateThankYouNoteDto
    {
        public string SenderId { get; set; }
        public string DonorId { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
    }

    public class MongoDB_UpdateThankYouNoteDto
    {
        public string Message { get; set; }
        public int Rating { get; set; }
    }

    public class MongoDB_ThankYouNoteDto
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string DonorId { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
    }
}
