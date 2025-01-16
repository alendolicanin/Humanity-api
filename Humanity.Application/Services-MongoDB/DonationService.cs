using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository;
using Humanity.Repository.MongoDB;

namespace Humanity.Application.Services_MongoDB
{
    public class DonationService
    {
        // Privatna polja za rad sa MongoDB i SQL UnitOfWork-om
        private readonly IMongoUnitOfWork _unitOfWork;
        private readonly IUnitOfWork _unitOfWork_SQL;

        public DonationService(IMongoUnitOfWork unitOfWork, IUnitOfWork unitOfWork_SQL)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork_SQL = unitOfWork_SQL;
        }

        // Metoda za dobijanje donacije po ID-u
        public async Task<MongoDB_DonationDto> GetDonationByIdAsync(string id)
        {
            var donation = await _unitOfWork.DonationRepository.GetById(id);
            if (donation == null)
            {
                throw new Exception("Donation not found.");
            }
            // Mapiranje entiteta na DTO objekat za povrat korisniku
            return new MongoDB_DonationDto
            {
                Id = donation.Id,
                DonorId = donation.DonorId,
                DateReceived = donation.DateReceived,
                Value = donation.Value,
                Category = (MongoDB_DonationCategory)(int)donation.Category, // Konverzija kategorije
                IsAnonymous = donation.IsAnonymous,
                IsDistributed = donation.IsDistributed
            };
        }

        // Metoda za dobijanje svih donacija
        public async Task<IEnumerable<MongoDB_DonationDto>> GetAllDonationsAsync()
        {
            var donations = await _unitOfWork.DonationRepository.GetAll();

            // Mapiranje kolekcije entiteta na kolekciju DTO objekata
            return donations.Select(donation => new MongoDB_DonationDto
            {
                Id = donation.Id,
                DonorId = donation.DonorId,
                DateReceived = donation.DateReceived,
                Value = donation.Value,
                Category = (MongoDB_DonationCategory)(int)donation.Category, // Konverzija kategorije
                IsAnonymous = donation.IsAnonymous,
                IsDistributed = donation.IsDistributed
            });
        }

        // Metoda za kreiranje nove donacije
        public async Task<Donation> CreateDonationAsync(MongoDB_CreateDonationDto createDonationDto)
        {
            // Dohvatanje informacije o donatoru iz SQL baze
            var donor = await _unitOfWork_SQL.UserRepository.GetById(createDonationDto.DonorId);
            if (donor == null)
            {
                throw new Exception("Donor not found.");
            }

            // Kreiranje novog entiteta donacije
            var donation = new Donation
            {
                DonorId = createDonationDto.DonorId,
                Value = createDonationDto.Value,
                Category = (Humanity.Infrastructure.MongoDB.Models.MongoDB.DonationCategory)(int)createDonationDto.Category,
                IsAnonymous = donor.IsAnonymous,
                IsDistributed = false, 
                DateReceived = DateTime.UtcNow
            };

            // Dodavanje donacije u MongoDB
            await _unitOfWork.DonationRepository.Add(donation);

            // Čuvanje promena u bazi podataka
            await _unitOfWork.CompleteAsync();

            return donation;
        }

        // Metoda za ažuriranje postojeće donacije
        public async Task<bool> UpdateDonationAsync(string id, MongoDB_UpdateDonationDto updateDonationDto)
        {
            // Dohvatanje donacije iz baze na osnovu ID-a
            var donation = await _unitOfWork.DonationRepository.GetById(id);
            if (donation == null) return false;

            // Ažuriranje polja donacije
            donation.Value = updateDonationDto.Value;
            donation.Category = (Humanity.Infrastructure.MongoDB.Models.MongoDB.DonationCategory)(int)updateDonationDto.Category;

            // Ažuriranje u repozitorijumu
            await _unitOfWork.DonationRepository.Update(donation);

            // Čuvanje promena
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // Metoda za brisanje donacije
        public async Task<bool> DeleteDonationAsync(string id)
        {
            // Dohvatanje donacije iz baze na osnovu ID-a
            var donation = await _unitOfWork.DonationRepository.GetById(id);
            if (donation == null) return false;

            // Brisanje donacije iz baze
            await _unitOfWork.DonationRepository.Delete(id);

            // Čuvanje promena
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }

    public class MongoDB_CreateDonationDto
    {
        public string DonorId { get; set; }
        public decimal Value { get; set; }
        public MongoDB_DonationCategory Category { get; set; }
    }

    public class MongoDB_DonationDto
    {
        public string Id { get; set; } 
        public string DonorId { get; set; }
        public DateTime DateReceived { get; set; }
        public decimal Value { get; set; }
        public MongoDB_DonationCategory Category { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsDistributed { get; set; }
    }

    public class MongoDB_UpdateDonationDto
    {
        public decimal Value { get; set; }
        public MongoDB_DonationCategory Category { get; set; }
    }

    public enum MongoDB_DonationCategory
    {
        Food, // Hrana 0
        Clothing, // Odeća 1
        Footwear, // Obuća 2
        Money // Novac 3
    }
}
