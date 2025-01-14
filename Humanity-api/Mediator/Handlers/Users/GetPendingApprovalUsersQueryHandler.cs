using Humanity.API.Mediator.Queries.Users;
using Humanity.Application.DTOs;
using Humanity.Application.Interfaces;
using MediatR;

namespace Humanity.API.Mediator.Handlers.Users
{
    public class GetPendingApprovalUsersQueryHandler : IRequestHandler<GetPendingApprovalUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserService _userService;

        public GetPendingApprovalUsersQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetPendingApprovalUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetPendingApprovalUsersAsync();
        }
    }
}
