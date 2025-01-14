using Humanity.Application.DTOs;

namespace Humanity.Application.Interfaces
{
    public interface IDonationService
    {
        Task<DonationDto> GetDonationByIdAsync(int id);
        Task<IEnumerable<DonationDto>> GetAllDonationsAsync(DonationQueryDto queryDto);
        Task<DonationDto> CreateDonationAsync(CreateDonationDto createDonationDto);
        Task<bool> UpdateDonationAsync(int id, UpdateDonationDto updateDonationDto); 
        Task<bool> DeleteDonationAsync(int id);
    }
}
