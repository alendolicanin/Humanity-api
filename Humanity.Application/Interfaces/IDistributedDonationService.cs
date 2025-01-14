using Humanity.Application.DTOs;

namespace Humanity.Application.Interfaces
{
    public interface IDistributedDonationService
    {
        Task<DistributedDonationDto> GetDistributedDonationByIdAsync(int id);
        Task<IEnumerable<DistributedDonationDto>> GetAllDistributedDonationsAsync();
        Task<DistributedDonationDto> CreateDistributedDonationAsync(CreateDistributedDonationDto createDistributedDonationDto);
        Task<bool> DeleteDistributedDonationAsync(int id);
    }
}
