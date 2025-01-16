using Humanity.Infrastructure.Neo4jDB.Models.Neo4jDB;
using Humanity.Repository.Neo4jDB;

namespace Humanity.Application.Services_Neo4jDB
{
    public class ThankYouNoteService
    {
        // Privatno polje za upravljanje transakcijama i repozitorijumima za Neo4j
        private readonly INeo4jUnitOfWork _unitOfWork;

        public ThankYouNoteService(INeo4jUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Metoda za preuzimanje zahvalnice na osnovu ID-a
        public async Task<Neo4j_ThankYouNoteDto> GetThankYouNoteByIdAsync(string id)
        {
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null)
            {
                throw new Exception("Thank you note not found.");
            }

            // Mapiranje zahvalnice u DTO objekat za prikaz korisniku
            return new Neo4j_ThankYouNoteDto
            {
                Id = thankYouNote.Id,
                SenderId = thankYouNote.SenderId,
                DonorId = thankYouNote.DonorId,
                Message = thankYouNote.Message,
                Rating = thankYouNote.Rating
            };
        }

        // Metoda za preuzimanje svih zahvalnica
        public async Task<IEnumerable<Neo4j_ThankYouNoteDto>> GetAllThankYouNotesAsync()
        {
            var thankYouNotes = await _unitOfWork.ThankYouNoteRepository.GetAll();

            // Mapiranje svake zahvalnice u DTO objekat
            return thankYouNotes.Select(thankYouNote => new Neo4j_ThankYouNoteDto
            {
                Id = thankYouNote.Id,
                SenderId = thankYouNote.SenderId,
                DonorId = thankYouNote.DonorId,
                Message = thankYouNote.Message,
                Rating = thankYouNote.Rating
            });
        }

        // Metoda za kreiranje nove zahvalnice
        public async Task<ThankYouNote> CreateThankYouNoteAsync(Neo4j_CreateThankYouNoteDto createThankYouNoteDto)
        {
            // Kreiranje novog entiteta zahvalnice
            var thankYouNote = new ThankYouNote
            {
                Id = Guid.NewGuid().ToString(), 
                SenderId = createThankYouNoteDto.SenderId,
                DonorId = createThankYouNoteDto.DonorId,
                Message = createThankYouNoteDto.Message,
                Rating = createThankYouNoteDto.Rating
            };

            // Dodavanje nove zahvalnice u Neo4j repozitorijum
            await _unitOfWork.ThankYouNoteRepository.Add(thankYouNote);

            await _unitOfWork.CompleteAsync();

            return thankYouNote;
        }

        // Metoda za ažuriranje postojeće zahvalnice
        public async Task<bool> UpdateThankYouNoteAsync(string id, Neo4j_UpdateThankYouNoteDto updateThankYouNoteDto)
        {
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null) return false;

            // Ažuriranje polja zahvalnice na osnovu prosleđenih podataka
            thankYouNote.Message = updateThankYouNoteDto.Message;
            thankYouNote.Rating = updateThankYouNoteDto.Rating;

            // Čuvanje ažurirane zahvalnice u repozitorijumu
            await _unitOfWork.ThankYouNoteRepository.Update(thankYouNote);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        // Metoda za brisanje zahvalnice na osnovu ID-a
        public async Task<bool> DeleteThankYouNoteAsync(string id)
        {
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null) return false;

            // Brisanje zahvalnice iz repozitorijuma
            await _unitOfWork.ThankYouNoteRepository.Delete(id);

            // Čuvanje promena u bazi podataka
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }

    public class Neo4j_CreateThankYouNoteDto
    {
        public string SenderId { get; set; }
        public string DonorId { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
    }

    public class Neo4j_UpdateThankYouNoteDto
    {
        public string Message { get; set; }
        public int Rating { get; set; }
    }

    public class Neo4j_ThankYouNoteDto
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string DonorId { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
    }
}
