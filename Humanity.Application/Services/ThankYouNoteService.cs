using AutoMapper;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using Humanity.Domain.Models;
using Humanity.Repository;

namespace Humanity.Application.Services
{
    public class ThankYouNoteService : IThankYouNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ThankYouNoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ThankYouNoteDto> GetThankYouNoteByIdAsync(int id)
        {
            // Koristimo relacijsku bazu podataka za preuzimanje zahvalnice
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            return _mapper.Map<ThankYouNoteDto>(thankYouNote);
        }

        public async Task<IEnumerable<ThankYouNoteDto>> GetAllThankYouNotesAsync()
        {
            // Koristimo relacijsku bazu podataka za preuzimanje svih zahvalnica
            var thankYouNotes = _unitOfWork.ThankYouNoteRepository.GetAll();
            return _mapper.Map<IEnumerable<ThankYouNoteDto>>(thankYouNotes);
        }

        public async Task<ThankYouNoteDto> CreateThankYouNoteAsync(CreateThankYouNoteDto createThankYouNoteDto)
        {
            // Kreiramo zahvalnicu u relacijskoj bazi podataka
            var thankYouNote = _mapper.Map<ThankYouNote>(createThankYouNoteDto);
            await _unitOfWork.ThankYouNoteRepository.Add(thankYouNote);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ThankYouNoteDto>(thankYouNote);
        }

        public async Task<bool> UpdateThankYouNoteAsync(int id, UpdateThankYouNoteDto updateThankYouNoteDto)
        {
            // Ažuriramo zahvalnicu u relacijskoj bazi podataka
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null) return false;
            _mapper.Map(updateThankYouNoteDto, thankYouNote);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteThankYouNoteAsync(int id)
        {
            // Brišemo zahvalnicu iz relacijske baze podataka
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null) return false;
            await _unitOfWork.ThankYouNoteRepository.Delete(thankYouNote);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
