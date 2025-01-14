using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Humanity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Kontroler za autentifikaciju korisnika
    public class AuthController : ControllerBase
    {
        // Servis za autentifikaciju i autorizaciju korisnika
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Registruje korisnika u sistemu
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                // Pozovi servis za registraciju korisnika
                await _authService.RegisterAsync(registerDto);
                return Ok(new { message = "Registration successful. Please wait for admin approval." });
            }
            catch (Exception ex)
            {
                // Vrati grešku
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        // Prijavi korisnika na sistem
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                // Pozovi servis za prijavu korisnika
                var result = await _authService.LoginAsync(loginDto);
                // Vrati rezultat
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Vrati grešku
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("change-password/{id}")]
        // Promeni lozinku korisnika
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordDto changePasswordDto)
        {
            try
            {
                // Pozovi servis za promenu lozinke
                var result = await _authService.ChangePasswordAsync(id, changePasswordDto);
                // Vrati rezultat
                if (!result)
                {
                    // Ukoliko je promena lozinke neuspešna, vrati grešku
                    return BadRequest("Failed to change password");
                }
                // Ukoliko je promena lozinke uspešna, vrati poruku o uspehu
                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                // Vrati grešku
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
