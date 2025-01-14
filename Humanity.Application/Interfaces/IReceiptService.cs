using Humanity.Application.DTOs;

namespace Humanity.Application.Interfaces
{
    public interface IReceiptService
    {
        Task<ReceiptDto> GetReceiptByIdAsync(int id);
        Task<IEnumerable<ReceiptDto>> GetAllReceiptsAsync();
        Task<bool> DeleteReceiptAsync(int id);
        Task<bool> ConfirmSignatureAsync(int receiptId, string recipientId);
    }
}
