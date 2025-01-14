using MediatR;

namespace Humanity.API.Mediator.Commands.Users
{
    public class ApproveUserCommand : IRequest<bool>
    {
        public string UserId { get; set; }
    }
}
