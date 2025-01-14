using Humanity.API.Mediator.Commands.Users;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Users
{
    public class RejectUserCommandHandler : IRequestHandler<RejectUserCommand, bool>
    {
        private readonly IUserService _userService;

        public RejectUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(RejectUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.RejectUserAsync(request.UserId);
        }
    }
}
