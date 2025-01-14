using Humanity.API.Mediator.Commands.Users;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Users
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserService _userService;

        public UpdateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.UpdateUserAsync(request.UserId, request.UpdateUserDto);
        }
    }
}
