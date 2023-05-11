using BS.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Features.Commands.Deposits
{
    public record DepositCommand(DepositRequest Request, string userId) : IRequest<APIResponse>;
}
