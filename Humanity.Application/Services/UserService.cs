using AutoMapper;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using Humanity.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Humanity.Application.Services
{
    public class UserService : IUserService 
    {
        // Upravljanje korisnicima, pruža funkcionalnosti kao što su kreiranje,
        // ažuriranje i brisanje korisnika
        private readonly UserManager<User> _userManager;

        // Automatsko mapiranje objekata, koristi se za konverziju između entiteta i DTOs
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        // Metoda za dobijanje korisnika po ID-u
        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            // Pronalazi korisnika po ID-u
            var user = await _userManager.FindByIdAsync(id);
            // Automatsko mapiranje korisnika u UserDto
            return _mapper.Map<UserDto>(user);
        }

        // Metoda za dobijanje svih korisnika
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            // Dobijanje svih korisnika
            var users = _userManager.Users;
            // Automatsko mapiranje korisnika u UserDto
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        // Metoda za kreiranje korisnika
        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Automatsko mapiranje CreateUserDto u User
            var user = _mapper.Map<User>(createUserDto);
            // Kreiranje korisnika
            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            // Provera da li je kreiranje uspešno
            if (!result.Succeeded)
            {
                // Ukoliko nije, prikaži greške
                throw new System.Exception("Failed to create user");
            }

            // Ako nema grešaka, automatsko mapiranje korisnika u UserDto
            return _mapper.Map<UserDto>(user);
        }

        // Metoda za ažuriranje korisnika
        public async Task<bool> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            // Pronalazi korisnika po ID-u
            var user = await _userManager.FindByIdAsync(id);
            // Provera da li je korisnik pronađen, ukoliko nije, vrati false
            if (user == null) return false;

            // Automatsko mapiranje UpdateUserDto u User
            _mapper.Map(updateUserDto, user);
            // Ažuriranje korisnika u bazi podataka
            var result = await _userManager.UpdateAsync(user);
            // Vraća rezultat ažuriranja, true ukoliko je uspešno, false ukoliko nije
            return result.Succeeded;
        }

        // Metoda za brisanje korisnika
        public async Task<bool> DeleteUserAsync(string id)
        {
            // Pronalazi korisnika po ID-u
            var user = await _userManager.FindByIdAsync(id);
            // Provera da li je korisnik pronađen, ukoliko nije, vrati false
            if (user == null) return false;

            // Brisanje korisnika iz baze podataka
            var result = await _userManager.DeleteAsync(user);
            // Vraća rezultat brisanja, true ukoliko je uspešno, false ukoliko nije
            return result.Succeeded;
        }

        // Odobrava (aktivira) korisnika
        public async Task<bool> ApproveUserAsync(string userId)
        {
            // Pronalazi korisnika po ID-u
            var user = await _userManager.FindByIdAsync(userId);
            // Provera da li je korisnik pronađen
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Postavlja korisnika kao aktivnog
            user.IsActive = true;
            // Ažurira korisnika u bazi podataka
            var result = await _userManager.UpdateAsync(user);

            // Provera da li je ažuriranje uspešno
            if (!result.Succeeded)
            {
                // Ukoliko nije, prikaži greške
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to approve user: {errors}");
            }

            // Ukoliko je sve uspešno, vrati true
            return true;
        }

        // Odbija korisnika
        public async Task<bool> RejectUserAsync(string userId)
        {
            // Pronalazi korisnika po ID-u
            var user = await _userManager.FindByIdAsync(userId);
            // Provera da li je korisnik pronađen
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Brisanje korisnika iz baze podataka
            var result = await _userManager.DeleteAsync(user);
            // Vraća rezultat brisanja, true ukoliko je uspešno, false ukoliko nije
            return result.Succeeded;
        }

        // Dobija listu korisnika koji čekaju na odobrenje
        public async Task<IEnumerable<UserDto>> GetPendingApprovalUsersAsync()
        {
            // Dobija listu korisnika koji nisu aktivni
            var users = await _userManager.Users.Where(u => !u.IsActive).ToListAsync();
            // Mapira listu korisnika u listu UserDto objekata
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
