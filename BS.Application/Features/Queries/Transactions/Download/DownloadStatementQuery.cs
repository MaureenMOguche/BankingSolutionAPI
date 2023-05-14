using BS.Application.Models;
using BS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Features.Queries.Transactions.Download
{
    public record DownloadStatementQuery(string userId) : IRequest<PDFReturn>;
}
