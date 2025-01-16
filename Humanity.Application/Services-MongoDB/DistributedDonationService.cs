using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository;
using Humanity.Repository.MongoDB;

namespace Humanity.Application.Services_MongoDB
{
    public class DistributedDonationService
    {
        // Privatna polja za rad sa MongoDB i SQL UnitOfWork-om
        private readonly IMongoUnitOfWork _unitOfWork;
        private readonly IUnitOfWork _unitOfWork_SQL;

        public DistributedDonationService(IMongoUnitOfWork unitOfWork, IUnitOfWork unitOfWork_SQL)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork_SQL = unitOfWork_SQL;
        }

        // Metoda za dobijanje distribuirane donacije na osnovu ID-a
        public async Task<MongoDB_DistributedDonationDto> GetDistributedDonationByIdAsync(string id)
        {
            var distributedDonation = await _unitOfWork.DistributedDonationRepository.GetById(id);
            // Mapiranje entiteta distribuirane donacije u DTO i vraćanje korisniku
            return new MongoDB_DistributedDonationDto
            {
                Id = distributedDonation.Id,
                DonationId = distributedDonation.DonationId,
                RecipientId = distributedDonation.RecipientId,
                DateDistributed = distributedDonation.DateDistributed,
                Value = distributedDonation.Value
            };
        }

        // Metoda za dobijanje svih distribuiranih donacija
        public async Task<IEnumerable<MongoDB_DistributedDonationDto>> GetAllDistributedDonationsAsync()
        {
            var distributedDonations = await _unitOfWork.DistributedDonationRepository.GetAll();
            // Mapiranje svih entiteta distribuiranih donacija u DTO objekte
            return distributedDonations.Select(distributedDonation => new MongoDB_DistributedDonationDto
            {
                Id = distributedDonation.Id,
                DonationId = distributedDonation.DonationId,
                RecipientId = distributedDonation.RecipientId,
                DateDistributed = distributedDonation.DateDistributed,
                Value = distributedDonation.Value
            });
        }

        // Metoda za kreiranje nove distribuirane donacije
        public async Task<MongoDB_DistributedDonationDto> CreateDistributedDonationAsync(MongoDB_CreateDistributedDonationDto createDistributedDonationDto)
        {
            // Dohvatanje originalne donacije iz MongoDB baze na osnovu ID-a
            var originalDonation = await _unitOfWork.DonationRepository.GetById(createDistributedDonationDto.DonationId);
            if (originalDonation == null)
            {
                throw new Exception("Original donation not found.");
            }

            // Dohvatanje primaoca iz SQL baze na osnovu ID-a
            var recipient = await _unitOfWork_SQL.UserRepository.GetById(createDistributedDonationDto.RecipientId);
            if (recipient == null)
            {
                throw new Exception("Recipient not found.");
            }

            // Dohvatanje donatora iz SQL baze na osnovu ID-a originalne donacije
            var donor = await _unitOfWork_SQL.UserRepository.GetById(originalDonation.DonorId);
            if (donor == null)
            {
                throw new Exception("Donor not found.");
            }

            // Provera da li je vrednost distribuirane donacije veća od dostupne vrednosti originalne donacije
            if (createDistributedDonationDto.Value > originalDonation.Value)
            {
                throw new Exception("Distributed value exceeds available donation value.");
            }

            // Ažuriranje vrednosti originalne donacije i statusa distribucije
            originalDonation.Value -= createDistributedDonationDto.Value;
            if (!originalDonation.IsDistributed)
            {
                originalDonation.IsDistributed = true; // Postavljanje statusa distribucije na true
            }
            // Ažuriranje originalne donacije
            await _unitOfWork.DonationRepository.Update(originalDonation);

            // Kreiranje novog entiteta distribuirane donacije
            var distributedDonation = new DistributedDonation
            {
                DonationId = originalDonation.Id,
                RecipientId = recipient.Id,
                DateDistributed = createDistributedDonationDto.DateDistributed,
                Value = createDistributedDonationDto.Value
            };

            distributedDonation.DonationId = originalDonation.Id;

            // Dodavanje distribuirane donacije u repozitorijum
            var addedDistributedDonation = await _unitOfWork.DistributedDonationRepository.Add(distributedDonation);
            await _unitOfWork.CompleteAsync(); // Čuvanje promena u bazi

            // Kreiranje računa za distribuiranu donaciju
            var receipt = new Receipt
            {
                DateIssued = DateTime.UtcNow,
                Value = createDistributedDonationDto.Value,
                DonorInfo = originalDonation.IsAnonymous ? "Anonymous" : $"{donor.FirstName} {donor.LastName}",
                RecipientInfo = $"{recipient.FirstName} {recipient.LastName}",
                Signature = $"{recipient.FirstName} {recipient.LastName}",
                DistributedDonationId = addedDistributedDonation.Id
            };

            // Dodavanje računa u repozitorijum
            await _unitOfWork.ReceiptRepository.Add(receipt);
            await _unitOfWork.CompleteAsync(); // Čuvanje promena u bazi

            // Mapiranje kreirane distribuirane donacije u DTO i vraćanje korisniku
            return new MongoDB_DistributedDonationDto
            {
                Id = addedDistributedDonation.Id,
                DonationId = addedDistributedDonation.DonationId,
                RecipientId = addedDistributedDonation.RecipientId,
                DateDistributed = addedDistributedDonation.DateDistributed,
                Value = addedDistributedDonation.Value
            };
        }

        // Metoda za brisanje distribuirane donacije
        public async Task<bool> DeleteDistributedDonationAsync(string id)
        {
            // Dohvatanje distribuirane donacije na osnovu ID-a
            var distributedDonation = await _unitOfWork.DistributedDonationRepository.GetById(id);
            if (distributedDonation == null)
            {
                return false;
            }

            // Brisanje distribuirane donacije iz repozitorijuma
            await _unitOfWork.DistributedDonationRepository.Delete(id);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }

    public class MongoDB_CreateDistributedDonationDto
    {
        public string DonationId { get; set; }
        public string RecipientId { get; set; }
        public DateTime DateDistributed { get; set; }
        public decimal Value { get; set; }
    }

    public class MongoDB_DistributedDonationDto
    {
        public string Id { get; set; }
        public string DonationId { get; set; }
        public string RecipientId { get; set; }
        public DateTime DateDistributed { get; set; }
        public decimal Value { get; set; }
    }
}
