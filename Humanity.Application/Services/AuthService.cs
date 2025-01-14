using AutoMapper;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using Humanity.Domain.Enums;
using Humanity.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Humanity.Application.Services
{
    // Servis za autentifikaciju i autorizaciju korisnika
    public class AuthService : IAuthService
    {
        // Upravljanje korisnicima, pruža funkcionalnosti kao što su kreiranje, ažuriranje i
        // brisanje korisnika
        private readonly UserManager<User> _userManager;

        // Automatsko mapiranje objekata, koristi se za konverziju između entiteta i DTOs
        private readonly IMapper _mapper;

        // Konfiguracija aplikacije, koristi se za pristup podešavanjima kao što su JWT ključevi
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        // Registruje novog korisnika u sistemu
        public async Task RegisterAsync(RegisterDto registerDto)
        {
            // Proveri validnost uloge
            if (!Enum.IsDefined(typeof(UserRole), registerDto.Role))
            {
                throw new ArgumentException("Invalid user role");
            }

            // Mapiranje podataka iz DTO-a u entitet korisnika
            var user = _mapper.Map<User>(registerDto);
            // Postavljanje korisničkog imena na email
            user.UserName = registerDto.Email;
            user.IsActive = false; // Korisnik je neaktivan dok ga admin ne potvrdi

            // Kreiranje korisnika u bazi podataka
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            // Provera da li je kreiranje korisnika uspešno
            if (!result.Succeeded)
            {
                // Ukoliko nije, prikaži greške
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to register user: {errors}");
            }
        }

        // Prijavljuje korisnika u sistem
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Pronalazi korisnika po email-u
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            // Provera da li korisnik postoji i da li je uneta ispravna lozinka
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new Exception("Invalid login attempt");
            }

            // Provera da li je korisnik aktivan
            if (!user.IsActive)
            {
                throw new Exception("User account is not active. Please contact the administrator.");
            }

            // Generisanje JWT tokena za korisnika
            var token = GenerateJwtToken(user);

            // Mapiranje korisnika u UserDto
            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Age = user.Age,
                Role = user.Role,
                IsActive = user.IsActive,
                IsAnonymous = user.IsAnonymous,
                Email = user.Email,
                Image = user.Image,
                RegisteredCategories = user.RegisteredCategories,
            };

            // Vraćanje AuthResponseDto sa tokenom, vremenom isteka i korisnikom
            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(2),
                User = userDto
            };
        }

        // Menja lozinku korisnika
        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            // Pronalazi korisnika po ID-ju
            var user = await _userManager.FindByIdAsync(userId);
            // Provera da li je korisnik pronađen
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Provera da li se nova lozinka i potvrdna lozinka podudaraju
            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
            {
                throw new Exception("New password and confirmation password do not match.");
            }

            // Promena lozinke korisnika
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            // Provera da li je promena lozinke uspešna
            if (!result.Succeeded)
            {
                // Ukoliko nije, prikaži greške
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to change password: {errors}");
            }

            // Ukoliko je promena lozinke uspešna, vrati true
            return true;
        }

        // Metoda za generisanje JWT tokena
        private string GenerateJwtToken(User user)
        {
            // Kreiranje ključa za potpisivanje tokena
            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Postavljanje podataka u JWT token
            var authClaims = new List<Claim>
            {
                // Postavljanje korisničkog imena, uloge i ID-ja u token
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            // Kreiranje JWT tokena
            var token = new JwtSecurityToken(
                claims: authClaims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256)
            );

            // Vraćanje JWT tokena kao string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
