using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Queries.Users
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public string UserId { get; set; }
    }
}
