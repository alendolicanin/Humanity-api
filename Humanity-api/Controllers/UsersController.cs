using Humanity.API.Mediator.Commands.Users;
using Humanity.API.Mediator.Queries.Users;
using Humanity.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Humanity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Akcija za kreiranje korisnika (Admin može kreirati korisnike)
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                var user = await _mediator.Send(new CreateUserCommand { CreateUserDto = createUserDto });
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za dobijanje korisnika po ID-u (Admin može videti korisnika po ID-u)
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByIdQuery { UserId = id });
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za dobijanje svih korisnika (Admin može videti sve korisnike)
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _mediator.Send(new GetAllUsersQuery());
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za ažuriranje korisnika (Admin može ažurirati korisnike)
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDto updateUserDto)
        {
            try
            {
                var result = await _mediator.Send(new UpdateUserCommand { UserId = id, UpdateUserDto = updateUserDto });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za brisanje korisnika (Admin može brisati korisnike)
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteUserCommand { UserId = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za odobravanje korisnika (Admin može odobravati korisnike)
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveUser(string id)
        {
            try
            {
                var result = await _mediator.Send(new ApproveUserCommand { UserId = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za odbijanje korisnika (Admin može odbijati korisnike)
        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectUser(string id)
        {
            try
            {
                var result = await _mediator.Send(new RejectUserCommand { UserId = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Akcija za dobijanje korisnika koji čekaju na odobrenje (Admin može videti korisnike koji čekaju na odobrenje)
        [HttpGet("pending-approval")]
        public async Task<IActionResult> GetPendingApprovalUsers()
        {
            try
            {
                var users = await _mediator.Send(new GetPendingApprovalUsersQuery());
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
