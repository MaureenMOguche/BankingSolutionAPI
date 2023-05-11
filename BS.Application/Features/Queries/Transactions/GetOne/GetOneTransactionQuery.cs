using BS.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Features.Queries.Transactions.GetOne
{
    public record GetOneTransactionQuery(string userId) : IRequest<APIResponse>;
}
