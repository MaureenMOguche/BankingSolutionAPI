using BS.Application.Features.Queries.Models;
using BS.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Features.Queries.Transactions.GetAll
{
    public record GetAllTransactionsQuery(QueryParameters QueryParameters, string userId) : IRequest<APIResponse>;
}
