using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Commands.Users
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public UpdateUserDto UpdateUserDto { get; set; }
    }
}
