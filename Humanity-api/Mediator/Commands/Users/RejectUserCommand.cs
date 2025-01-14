using MediatR;

namespace Humanity.API.Mediator.Commands.Users
{
    public class RejectUserCommand : IRequest<bool>
    {
        public string UserId { get; set; }
    }
}
