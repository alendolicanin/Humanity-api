using Humanity.Infrastructure.Neo4jDB.Models.Neo4jDB;
using Humanity.Repository.Neo4jDB;

namespace Humanity.Application.Services_Neo4jDB
{
    public class ThankYouNoteService
    {
        private readonly INeo4jUnitOfWork _unitOfWork;

        public ThankYouNoteService(INeo4jUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Neo4j_ThankYouNoteDto> GetThankYouNoteByIdAsync(string id)
        {
            // Fetch the thank-you note by ID using Neo4j repository
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null)
            {
                throw new Exception("Thank you note not found.");
            }

            return new Neo4j_ThankYouNoteDto
            {
                Id = thankYouNote.Id,
                SenderId = thankYouNote.SenderId,
                DonorId = thankYouNote.DonorId,
                Message = thankYouNote.Message,
                Rating = thankYouNote.Rating
            };
        }

        public async Task<IEnumerable<Neo4j_ThankYouNoteDto>> GetAllThankYouNotesAsync()
        {
            // Fetch all thank-you notes using Neo4j repository
            var thankYouNotes = await _unitOfWork.ThankYouNoteRepository.GetAll();

            return thankYouNotes.Select(thankYouNote => new Neo4j_ThankYouNoteDto
            {
                Id = thankYouNote.Id,
                SenderId = thankYouNote.SenderId,
                DonorId = thankYouNote.DonorId,
                Message = thankYouNote.Message,
                Rating = thankYouNote.Rating
            });
        }

        public async Task<ThankYouNote> CreateThankYouNoteAsync(Neo4j_CreateThankYouNoteDto createThankYouNoteDto)
        {
            // Create a new thank-you note
            var thankYouNote = new ThankYouNote
            {
                Id = Guid.NewGuid().ToString(), // Generate unique ID
                SenderId = createThankYouNoteDto.SenderId,
                DonorId = createThankYouNoteDto.DonorId,
                Message = createThankYouNoteDto.Message,
                Rating = createThankYouNoteDto.Rating
            };

            // Add the new thank-you note to the repository
            await _unitOfWork.ThankYouNoteRepository.Add(thankYouNote);

            // Commit the changes to the database
            await _unitOfWork.CompleteAsync();

            return thankYouNote;
        }

        public async Task<bool> UpdateThankYouNoteAsync(string id, Neo4j_UpdateThankYouNoteDto updateThankYouNoteDto)
        {
            // Fetch the thank-you note by ID
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null) return false;

            // Update the fields
            thankYouNote.Message = updateThankYouNoteDto.Message;
            thankYouNote.Rating = updateThankYouNoteDto.Rating;

            // Call the repository's Update method to persist the changes
            await _unitOfWork.ThankYouNoteRepository.Update(thankYouNote);

            // Commit the changes to the database
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteThankYouNoteAsync(string id)
        {
            // Fetch the thank-you note by ID
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null) return false;

            // Delete the thank-you note from the repository
            await _unitOfWork.ThankYouNoteRepository.Delete(id);

            // Commit the changes to the database
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
