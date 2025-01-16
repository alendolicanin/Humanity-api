using AutoMapper;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using Humanity.Domain.Models;
using Humanity.Repository;

namespace Humanity.Application.Services
{
    public class DistributedDonationService : IDistributedDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DistributedDonationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DistributedDonationDto> GetDistributedDonationByIdAsync(int id)
        {
            // Koristimo relacijsku bazu podataka za preuzimanje podeljene donacije po ID-u
            var distributedDonation = await _unitOfWork.DistributedDonationRepository.GetById(id);
            if (distributedDonation == null)
            {
                throw new Exception("Distributed donation not found.");
            }
            return _mapper.Map<DistributedDonationDto>(distributedDonation);
        }

        public async Task<IEnumerable<DistributedDonationDto>> GetAllDistributedDonationsAsync()
        {
            // Koristimo relacijsku bazu podataka za preuzimanje svih podeljenih donacija
            var distributedDonations = _unitOfWork.DistributedDonationRepository.GetAll();
            return _mapper.Map<IEnumerable<DistributedDonationDto>>(distributedDonations);
        }

        public async Task<DistributedDonationDto> CreateDistributedDonationAsync(CreateDistributedDonationDto createDistributedDonationDto)
        {
            // Dohvatamo originalnu donaciju iz baze podataka
            var originalDonation = await _unitOfWork.DonationRepository.GetById(createDistributedDonationDto.DonationId);
            if (originalDonation == null)
            {
                throw new Exception("Original donation not found.");
            }

            // Dohvatamo informacije o primaocu
            var recipient = await _unitOfWork.UserRepository.GetById(createDistributedDonationDto.RecipientId);
            if (recipient == null)
            {
                throw new Exception("Recipient not found.");
            }

            // Dohvatamo informacije o donatoru
            var donor = await _unitOfWork.UserRepository.GetById(originalDonation.DonorId);
            if (donor == null)
            {
                throw new Exception("Donor not found.");
            }

            // Proveravamo da li je vrednost za distribuciju validna
            if (createDistributedDonationDto.Value > originalDonation.Value)
            {
                throw new Exception("Distributed value exceeds available donation value.");
            }

            // Ažuriramo vrednost originalne donacije i postavljamo IsDistributed na true
            originalDonation.Value -= createDistributedDonationDto.Value;
            if (!originalDonation.IsDistributed)
            {
                originalDonation.IsDistributed = true;
            }
            await _unitOfWork.DonationRepository.Update(originalDonation);

            // Kreiramo distribuiranu donaciju za bazu
            var distributedDonation = _mapper.Map<DistributedDonation>(createDistributedDonationDto);
            distributedDonation.DonationId = originalDonation.Id; // Koristimo pravi ID donacije za SQL vezu
            //  Kako bih osigurao da polje DonationId u entitetu distribuirane donacije tačno ukazuje na ID originalne donacije

            // Čuvamo distribuiranu donaciju u relacijskoj bazi podataka (ID se generiše nakon dodavanja)
            var addedDistributedDonation = await _unitOfWork.DistributedDonationRepository.Add(distributedDonation);
            await _unitOfWork.CompleteAsync();

            // Kreiramo račun koristeći podatke iz distribuirane donacije
            var sqlReceipt = new Receipt
            {
                DateIssued = DateTime.UtcNow, // Postavljamo trenutni datum i vreme
                Value = createDistributedDonationDto.Value, // Koristimo istu vrednost kao i za distribuiranu donaciju
                DonorInfo = originalDonation.IsAnonymous ? "Anonymous" : $"{donor.FirstName} {donor.LastName}", // Ako je donacija anonimna, stavljamo "Anonymous", u suprotnom ime i prezime donatora
                RecipientInfo = $"{recipient.FirstName} {recipient.LastName}", // Ime i prezime primaoca
                Signature = "Nema potpisa", // Postavljamo podrazumevanu vrednost za potpis
                DistributedDonationId = addedDistributedDonation.Id // Koristimo stvarni ID distribuirane donacije za SQL vezu
            };

            // Čuvamo račun u SQL bazi podataka
            await _unitOfWork.ReceiptRepository.Add(sqlReceipt);

            // Potvrđujemo sve promene
            await _unitOfWork.CompleteAsync();

            // Mapiramo i distribuiranu donaciju i račun u DTO objekat
            var result = _mapper.Map<DistributedDonationDto>(addedDistributedDonation);
            result.Receipt = _mapper.Map<ReceiptDto>(sqlReceipt);

            return result;
        }

        public async Task<bool> DeleteDistributedDonationAsync(int id)
        {
            // Brišemo podeljenu donaciju iz relacijske baze podataka
            var distributedDonation = await _unitOfWork.DistributedDonationRepository.GetById(id);
            if (distributedDonation == null)
            {
                return false;
            }

            await _unitOfWork.DistributedDonationRepository.Delete(distributedDonation);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
