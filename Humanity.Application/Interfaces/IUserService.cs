using Humanity.Application.DTOs;

namespace Humanity.Application.Interfaces
{
    public interface IUserService
    {
        // Metoda za dobijanje korisnika po ID-u, vraća UserDto
        Task<UserDto> GetUserByIdAsync(string id);

        // Metoda za dobijanje svih korisnika, vraća listu UserDto
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        
        // Metoda za kreiranje korisnika, vraća UserDto
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);

        // Metoda za ažuriranje korisnika, vraća true ako je ažuriranje uspešno, inače false
        Task<bool> UpdateUserAsync(string id, UpdateUserDto updateUserDto);

        // Metoda za brisanje korisnika, vraća true ako je brisanje uspešno, inače false
        Task<bool> DeleteUserAsync(string id);

        // Metoda za odobravanje korisnika, vraća true ako je odobravanje uspešno, inače false
        Task<bool> ApproveUserAsync(string userId);
        
        // Metoda za odbijanje korisnika, vraća true ako je odbijanje uspešno, inače false
        Task<bool> RejectUserAsync(string userId);

        // Metoda za dobijanje korisnika koji čekaju na odobrenje, vraća listu UserDto
        Task<IEnumerable<UserDto>> GetPendingApprovalUsersAsync();
    }
}
