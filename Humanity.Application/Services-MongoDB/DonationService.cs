using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository;
using Humanity.Repository.MongoDB;

namespace Humanity.Application.Services_MongoDB
{
    public class DonationService
    {
        private readonly IMongoUnitOfWork _unitOfWork;
        private readonly IUnitOfWork _unitOfWork_SQL;

        public DonationService(IMongoUnitOfWork unitOfWork, IUnitOfWork unitOfWork_SQL)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork_SQL = unitOfWork_SQL;
        }

        public async Task<MongoDB_DonationDto> GetDonationByIdAsync(string id)
        {
            // Fetch the donation by ID using MongoDB repository
            var donation = await _unitOfWork.DonationRepository.GetById(id);
            if (donation == null)
            {
                throw new Exception("Donation not found.");
            }
            return new MongoDB_DonationDto
            {
                Id = donation.Id,
                DonorId = donation.DonorId,
                DateReceived = donation.DateReceived,
                Value = donation.Value,
                Category = (MongoDB_DonationCategory)(int)donation.Category,
                IsAnonymous = donation.IsAnonymous,
                IsDistributed = donation.IsDistributed
            };
        }

        public async Task<IEnumerable<MongoDB_DonationDto>> GetAllDonationsAsync()
        {
            // Fetch all donations using MongoDB repository
            var donations = await _unitOfWork.DonationRepository.GetAll();
            return donations.Select(donation => new MongoDB_DonationDto
            {
                Id = donation.Id,
                DonorId = donation.DonorId,
                DateReceived = donation.DateReceived,
                Value = donation.Value,
                Category = (MongoDB_DonationCategory)(int)donation.Category,
                IsAnonymous = donation.IsAnonymous,
                IsDistributed = donation.IsDistributed
            });
        }

        public async Task<Donation> CreateDonationAsync(MongoDB_CreateDonationDto createDonationDto)
        {
            // Dohvatamo informacije o donoru
            var donor = await _unitOfWork_SQL.UserRepository.GetById(createDonationDto.DonorId);
            if (donor == null)
            {
                throw new Exception("Donor not found.");
            }

            // Create a new donation
            var donation = new Donation
            {
                DonorId = createDonationDto.DonorId,
                Value = createDonationDto.Value,
                Category = (Humanity.Infrastructure.MongoDB.Models.MongoDB.DonationCategory)(int)createDonationDto.Category,
                IsAnonymous = donor.IsAnonymous,
                IsDistributed = false, // Initial value
                DateReceived = DateTime.UtcNow
            };

            // Add the new donation to the repository
            await _unitOfWork.DonationRepository.Add(donation);

            // Commit the changes to the database
            await _unitOfWork.CompleteAsync();

            return donation;
        }

        public async Task<bool> UpdateDonationAsync(string id, MongoDB_UpdateDonationDto updateDonationDto)
        {
            // Fetch the donation by ID
            var donation = await _unitOfWork.DonationRepository.GetById(id);
            if (donation == null) return false;

            // Update the fields
            donation.Value = updateDonationDto.Value;
            donation.Category = (Humanity.Infrastructure.MongoDB.Models.MongoDB.DonationCategory)(int)updateDonationDto.Category;

            // Call the repository's Update method to persist the changes
            await _unitOfWork.DonationRepository.Update(donation);

            // Commit the changes to the database
            await _unitOfWork.DonationRepository.Update(donation);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteDonationAsync(string id)
        {
            // Fetch the donation by ID
            var donation = await _unitOfWork.DonationRepository.GetById(id);
            if (donation == null) return false;

            // Delete the donation from the repository
            await _unitOfWork.DonationRepository.Delete(id);

            // Commit the changes to the database
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
