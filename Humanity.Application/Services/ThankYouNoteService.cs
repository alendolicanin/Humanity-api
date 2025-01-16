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
            // Mapiramo zahvalnicu iz entiteta ThankYouNote u DTO objekat ThankYouNoteDto
            return _mapper.Map<ThankYouNoteDto>(thankYouNote);
        }

        public async Task<IEnumerable<ThankYouNoteDto>> GetAllThankYouNotesAsync()
        {
            // Koristimo relacijsku bazu podataka za preuzimanje svih zahvalnica
            var thankYouNotes = _unitOfWork.ThankYouNoteRepository.GetAll();
            // Mapiramo kolekciju zahvalnica iz entiteta ThankYouNote u kolekciju DTO objekata ThankYouNoteDto
            return _mapper.Map<IEnumerable<ThankYouNoteDto>>(thankYouNotes);
        }

        public async Task<ThankYouNoteDto> CreateThankYouNoteAsync(CreateThankYouNoteDto createThankYouNoteDto)
        {
            // Mapiramo DTO objekat CreateThankYouNoteDto u entitet ThankYouNote
            var thankYouNote = _mapper.Map<ThankYouNote>(createThankYouNoteDto);
            // Dodajemo novu zahvalnicu u repozitorijum
            await _unitOfWork.ThankYouNoteRepository.Add(thankYouNote);

            // Čuvamo promene u bazi podataka (commit transakcije)
            await _unitOfWork.CompleteAsync();

            // Mapiramo kreiranu zahvalnicu nazad u DTO objekat ThankYouNoteDto
            return _mapper.Map<ThankYouNoteDto>(thankYouNote);
        }

        public async Task<bool> UpdateThankYouNoteAsync(int id, UpdateThankYouNoteDto updateThankYouNoteDto)
        {
            // Preuzimamo zahvalnicu iz baze podataka na osnovu njenog ID-a
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null) return false;
            // Mapiramo svojstva iz DTO objekta u postojeći entitet zahvalnice
            _mapper.Map(updateThankYouNoteDto, thankYouNote);

            // Čuvamo promene u bazi podataka
            await _unitOfWork.CompleteAsync(); // Ažuriranje se dešava implicitno – nije potrebno ručno pozivati metodu Update jer ORM prepoznaje promene na entitetima koji su preuzeti iz baze

            // Vraćamo true jer je operacija uspešno izvršena
            return true;
        }

        public async Task<bool> DeleteThankYouNoteAsync(int id)
        {
            // Preuzimamo zahvalnicu iz baze podataka na osnovu njenog ID-a
            var thankYouNote = await _unitOfWork.ThankYouNoteRepository.GetById(id);
            if (thankYouNote == null) return false;

            // Brišemo zahvalnicu iz repozitorijuma
            await _unitOfWork.ThankYouNoteRepository.Delete(thankYouNote);

            // Čuvamo promene u bazi podataka
            await _unitOfWork.CompleteAsync();

            // Vraćamo true jer je operacija uspešno izvršena
            return true;
        }
    }
}
