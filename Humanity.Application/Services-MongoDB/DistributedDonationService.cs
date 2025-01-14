using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository;
using Humanity.Repository.MongoDB;

namespace Humanity.Application.Services_MongoDB
{
    public class DistributedDonationService
    {
        private readonly IMongoUnitOfWork _unitOfWork;
        private readonly IUnitOfWork _unitOfWork_SQL;

        public DistributedDonationService(IMongoUnitOfWork unitOfWork, IUnitOfWork unitOfWork_SQL)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork_SQL = unitOfWork_SQL;
        }

        public async Task<MongoDB_DistributedDonationDto> GetDistributedDonationByIdAsync(string id)
        {
            var distributedDonation = await _unitOfWork.DistributedDonationRepository.GetById(id);
            return new MongoDB_DistributedDonationDto
            {
                Id = distributedDonation.Id,
                DonationId = distributedDonation.DonationId,
                RecipientId = distributedDonation.RecipientId,
                DateDistributed = distributedDonation.DateDistributed,
                Value = distributedDonation.Value
            };
        }

        public async Task<IEnumerable<MongoDB_DistributedDonationDto>> GetAllDistributedDonationsAsync()
        {
            var distributedDonations = await _unitOfWork.DistributedDonationRepository.GetAll();
            return distributedDonations.Select(distributedDonation => new MongoDB_DistributedDonationDto
            {
                Id = distributedDonation.Id,
                DonationId = distributedDonation.DonationId,
                RecipientId = distributedDonation.RecipientId,
                DateDistributed = distributedDonation.DateDistributed,
                Value = distributedDonation.Value
            });
        }

        public async Task<MongoDB_DistributedDonationDto> CreateDistributedDonationAsync(MongoDB_CreateDistributedDonationDto createDistributedDonationDto)
        {
            // Fetch the original donation
            var originalDonation = await _unitOfWork.DonationRepository.GetById(createDistributedDonationDto.DonationId);
            if (originalDonation == null)
            {
                throw new Exception("Original donation not found.");
            }

            // Fetch the recipient
            var recipient = await _unitOfWork_SQL.UserRepository.GetById(createDistributedDonationDto.RecipientId);
            if (recipient == null)
            {
                throw new Exception("Recipient not found.");
            }

            // Fetch the donor
            var donor = await _unitOfWork_SQL.UserRepository.GetById(originalDonation.DonorId);
            if (donor == null)
            {
                throw new Exception("Donor not found.");
            }

            // Validate the distributed value
            if (createDistributedDonationDto.Value > originalDonation.Value)
            {
                throw new Exception("Distributed value exceeds available donation value.");
            }

            // Update the original donation's value and distributed status
            originalDonation.Value -= createDistributedDonationDto.Value;
            if (!originalDonation.IsDistributed)
            {
                originalDonation.IsDistributed = true;
            }
            await _unitOfWork.DonationRepository.Update(originalDonation);

            // Create the distributed donation
            var distributedDonation = new DistributedDonation
            {
                DonationId = originalDonation.Id,
                RecipientId = recipient.Id,
                DateDistributed = createDistributedDonationDto.DateDistributed,
                Value = createDistributedDonationDto.Value
            };

            distributedDonation.DonationId = originalDonation.Id;

            // Add the distributed donation to the repository
            var addedDistributedDonation = await _unitOfWork.DistributedDonationRepository.Add(distributedDonation);
            await _unitOfWork.CompleteAsync();

            // Create the receipt
            var receipt = new Receipt
            {
                DateIssued = DateTime.UtcNow,
                Value = createDistributedDonationDto.Value,
                DonorInfo = originalDonation.IsAnonymous ? "Anonymous" : $"{donor.FirstName} {donor.LastName}",
                RecipientInfo = $"{recipient.FirstName} {recipient.LastName}",
                Signature = $"{recipient.FirstName} {recipient.LastName}",
                DistributedDonationId = addedDistributedDonation.Id
            };

            // Add the receipt to the repository
            await _unitOfWork.ReceiptRepository.Add(receipt);
            await _unitOfWork.CompleteAsync();

            // Map and return the result
            return new MongoDB_DistributedDonationDto
            {
                Id = addedDistributedDonation.Id,
                DonationId = addedDistributedDonation.DonationId,
                RecipientId = addedDistributedDonation.RecipientId,
                DateDistributed = addedDistributedDonation.DateDistributed,
                Value = addedDistributedDonation.Value
            };
        }

        public async Task<bool> DeleteDistributedDonationAsync(string id)
        {
            var distributedDonation = await _unitOfWork.DistributedDonationRepository.GetById(id);
            if (distributedDonation == null)
            {
                return false;
            }

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
