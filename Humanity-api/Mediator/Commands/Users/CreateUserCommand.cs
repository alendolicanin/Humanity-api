using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Commands.Users
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public CreateUserDto CreateUserDto { get; set; }
    }
}
