using AutoMapper;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using Humanity.Repository;

namespace Humanity.Application.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReceiptDto> GetReceiptByIdAsync(int id)
        {
            // Koristimo relacijsku bazu podataka za preuzimanje računa
            var receipt = await _unitOfWork.ReceiptRepository.GetById(id);
            return _mapper.Map<ReceiptDto>(receipt);
        }

        public async Task<IEnumerable<ReceiptDto>> GetAllReceiptsAsync()
        {
            // Koristimo relacijsku bazu podataka za preuzimanje svih računa
            var receipts = await _unitOfWork.ReceiptRepository.GetAll();
            return _mapper.Map<IEnumerable<ReceiptDto>>(receipts);
        }

        public async Task<bool> ConfirmSignatureAsync(int receiptId, string recipientId)
        {
            // Pronalazimo račun
            var receipt = await _unitOfWork.ReceiptRepository.GetById(receiptId);
            if (receipt == null)
                throw new Exception("Receipt not found.");

            // Pronalazimo primaoca
            var recipient = await _unitOfWork.UserRepository.GetById(recipientId);
            if (recipient == null)
                throw new Exception("Recipient not found.");

            // Ažuriramo potpis
            receipt.Signature = $"{recipient.FirstName} {recipient.LastName}";
            await _unitOfWork.ReceiptRepository.Update(receipt);
            await _unitOfWork.CompleteAsync();

            return true; // Potvrđujemo da je potpis uspešno dodat
        }

        public async Task<bool> DeleteReceiptAsync(int id)
        {
            // Brišemo račun iz relacijske baze podataka
            var receipt = await _unitOfWork.ReceiptRepository.GetById(id);
            if (receipt == null) return false;
            await _unitOfWork.ReceiptRepository.Delete(receipt);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
