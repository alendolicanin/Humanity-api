using Humanity.Infrastructure.MongoDB.Models.MongoDB;
using Humanity.Repository.MongoDB;

namespace Humanity.Application.Services_MongoDB
{
    public class ReceiptService
    {
        // Privatno polje za upravljanje transakcijama i repozitorijumima
        private readonly IMongoUnitOfWork _unitOfWork;

        public ReceiptService(IMongoUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; // Povezivanje sa UnitOfWork-om
        }

        // Metoda za preuzimanje računa na osnovu ID-a
        public async Task<MongoDB_ReceiptDto> GetReceiptByIdAsync(string id)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetById(id);
            // Mapiranje entiteta na DTO objekat i vraćanje korisniku
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

        // Metoda za preuzimanje svih računa
        public async Task<IEnumerable<MongoDB_ReceiptDto>> GetAllReceiptsAsync()
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAll();
            // Mapiranje liste entiteta na listu DTO objekata
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

        // Metoda za brisanje računa
        public async Task<bool> DeleteReceiptAsync(string id)
        {
            // Dohvatanje računa na osnovu ID-a
            var receipt = await _unitOfWork.ReceiptRepository.GetById(id);
            if (receipt == null) return false;

            // Brisanje računa iz repozitorijuma
            await _unitOfWork.ReceiptRepository.Delete(id);

            // Čuvanje promena u bazi podataka
            await _unitOfWork.CompleteAsync();

            return true; // Uspešno brisanje
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
