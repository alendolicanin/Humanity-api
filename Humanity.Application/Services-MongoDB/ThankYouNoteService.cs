using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository.MongoDB;

namespace Humanity.Application.Services_MongoDB
{
    public class ThankYouNoteService
    {
        private readonly IMongoUnitOfWork _unitOfWork;

        public ThankYouNoteService(IMongoUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MongoDB_ThankYouNoteDto> GetThankYouNoteByIdAsync(string id)
        {
            // Fetch the thank-you note by ID using MongoDB repository
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            return new MongoDB_ThankYouNoteDto
            {
                Id = thankYouNote.Id,
                SenderId = thankYouNote.SenderId,
                DonorId = thankYouNote.DonorId,
                Message = thankYouNote.Message,
                Rating = thankYouNote.Rating
            };
        }

        public async Task<IEnumerable<MongoDB_ThankYouNoteDto>> GetAllThankYouNotesAsync()
        {
            // Fetch all thank-you notes using MongoDB repository
            var thankYouNotes = await _unitOfWork.ThankYouNoteRepository.GetAll();
            return thankYouNotes.Select(thankYouNote => new MongoDB_ThankYouNoteDto
            {
                Id = thankYouNote.Id,
                SenderId = thankYouNote.SenderId,
                DonorId = thankYouNote.DonorId,
                Message = thankYouNote.Message,
                Rating = thankYouNote.Rating
            });
        }

        public async Task<ThankYouNote> CreateThankYouNoteAsync(MongoDB_CreateThankYouNoteDto createThankYouNoteDto)
        {
            // Create a new thank-you note
            var thankYouNote = new ThankYouNote
            {
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

        public async Task<bool> UpdateThankYouNoteAsync(string id, MongoDB_UpdateThankYouNoteDto updateThankYouNoteDto)
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
