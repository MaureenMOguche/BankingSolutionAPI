using BS.Application.Models;
using MediatR;

namespace BS.Application.Features.Commands.BankUsrs.UpdateUsers
{
    public record UpdateUserCommand(UpdateUserDto UpdateUser, string userId) : IRequest<APIResponse>
    {
    }
}
