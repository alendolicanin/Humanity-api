﻿using Humanity.Application.DTOs;
using MediatR;

namespace Humanity.API.Mediator.Queries.Users
{
    public class GetPendingApprovalUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }
}
