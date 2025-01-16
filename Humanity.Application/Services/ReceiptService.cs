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
            // Mapiramo entitet Receipt na DTO ReceiptDto i vraćamo rezultat
            return _mapper.Map<ReceiptDto>(receipt);
        }

        public async Task<IEnumerable<ReceiptDto>> GetAllReceiptsAsync()
        {
            // Koristimo relacijsku bazu podataka za preuzimanje svih računa
            var receipts = await _unitOfWork.ReceiptRepository.GetAll();
            // Mapiramo kolekciju entiteta Receipt na kolekciju DTO objekata ReceiptDto i vraćamo rezultat
            return _mapper.Map<IEnumerable<ReceiptDto>>(receipts);
        }

        // Metoda za potvrđivanje potpisa na računu
        public async Task<bool> ConfirmSignatureAsync(int receiptId, string recipientId)
        {
            // Preuzimamo račun na osnovu ID-a
            var receipt = await _unitOfWork.ReceiptRepository.GetById(receiptId);
            if (receipt == null)
                throw new Exception("Receipt not found.");

            // Preuzimamo korisnika (primaoca) na osnovu njegovog ID-a
            var recipient = await _unitOfWork.UserRepository.GetById(recipientId);
            if (recipient == null)
                throw new Exception("Recipient not found.");

            // Postavljamo potpis računa koristeći ime i prezime primaoca
            receipt.Signature = $"{recipient.FirstName} {recipient.LastName}";
            // Ažuriramo račun u repozitorijumu
            await _unitOfWork.ReceiptRepository.Update(receipt);
            // Čuvamo promene u bazi podataka
            await _unitOfWork.CompleteAsync();

            return true; // Potvrđujemo da je potpis uspešno dodat
        }

        public async Task<bool> DeleteReceiptAsync(int id)
        {
            // Preuzimamo račun na osnovu ID-a
            var receipt = await _unitOfWork.ReceiptRepository.GetById(id);
            if (receipt == null) return false;

            // Brišemo račun iz repozitorijuma
            await _unitOfWork.ReceiptRepository.Delete(receipt);
            // Čuvamo promene u bazi podataka
            await _unitOfWork.CompleteAsync();

            return true; // Vraćamo true kao potvrdu da je račun uspešno obrisan
        }
    }
}
