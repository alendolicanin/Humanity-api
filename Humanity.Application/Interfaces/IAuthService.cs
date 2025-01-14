using Humanity.Application.DTOs;

namespace Humanity.Application.Interfaces
{
    // Interfejs koji definiše servise za autentifikaciju i autorizaciju korisnika
    public interface IAuthService
    {
        // registerDto - Objekat koji sadrži informacije potrebne za registraciju korisnika
        Task RegisterAsync(RegisterDto registerDto);
        
        // loginDto - Objekat koji sadrži informacije potrebne za prijavu korisnika
        // Vraća objekat sa JWT tokenom i vremenom isteka tokena
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        
        // changePasswordDto - Objekat koji sadrži informacije potrebne za promenu lozinke korisnika
        // Vraća true ako je lozinka uspešno promenjena, u suprotnom false
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
    }
}
