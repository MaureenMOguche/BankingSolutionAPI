using BS.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Features.Commands.Transfers
{
    public record TransferCommand(TransferRequest Request, string userId) : IRequest<APIResponse>;
}
