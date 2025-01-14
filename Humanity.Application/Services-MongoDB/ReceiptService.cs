using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository.MongoDB;

namespace Humanity.Application.Services_MongoDB
{
    public class ReceiptService
    {
        private readonly IMongoUnitOfWork _unitOfWork;

        public ReceiptService(IMongoUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MongoDB_ReceiptDto> GetReceiptByIdAsync(string id)
        {
            // Fetch the receipt by ID using MongoDB repository
            var receipt = await _unitOfWork.ReceiptRepository.GetById(id);
            return new MongoDB_ReceiptDto
            {
                Id = receipt.Id,
                DistributedDonationId = receipt.DistributedDonationId,
                DateIssued = receipt.DateIssued,
                Value = receipt.Value,
                DonorInfo = receipt.DonorInfo,
                RecipientInfo = receipt.RecipientInfo,
                Signature = receipt.Signature
            };
        }

        public async Task<IEnumerable<MongoDB_ReceiptDto>> GetAllReceiptsAsync()
        {
            // Fetch all receipts using MongoDB repository
            var receipts = await _unitOfWork.ReceiptRepository.GetAll();
            return receipts.Select(receipt => new MongoDB_ReceiptDto
            {
                Id = receipt.Id,
                DistributedDonationId = receipt.DistributedDonationId,
                DateIssued = receipt.DateIssued,
                Value = receipt.Value,
                DonorInfo = receipt.DonorInfo,
                RecipientInfo = receipt.RecipientInfo,
                Signature = receipt.Signature
            });
        }

        public async Task<bool> DeleteReceiptAsync(string id)
        {
            // Fetch the receipt by ID
            var receipt = await _unitOfWork.ReceiptRepository.GetById(id);
            if (receipt == null) return false;

            // Delete the receipt from the repository
            await _unitOfWork.ReceiptRepository.Delete(id);

            // Commit the changes to the database
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }

    public class MongoDB_ReceiptDto
    {
        public string Id { get; set; }
        public string DistributedDonationId { get; set; }
        public DateTime DateIssued { get; set; }
        public decimal Value { get; set; }
        public string DonorInfo { get; set; }
        public string RecipientInfo { get; set; }
        public string Signature { get; set; }
    }
}
