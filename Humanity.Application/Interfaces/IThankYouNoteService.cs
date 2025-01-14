using Humanity.Application.DTOs;

namespace Humanity.Application.Interfaces
{
    public interface IThankYouNoteService
    {
        Task<ThankYouNoteDto> GetThankYouNoteByIdAsync(int id);
        Task<IEnumerable<ThankYouNoteDto>> GetAllThankYouNotesAsync();
        Task<ThankYouNoteDto> CreateThankYouNoteAsync(CreateThankYouNoteDto createThankYouNoteDto);
        Task<bool> UpdateThankYouNoteAsync(int id, UpdateThankYouNoteDto updateThankYouNoteDto);
        Task<bool> DeleteThankYouNoteAsync(int id);
    }
}
