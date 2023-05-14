using BS.Application.Contracts.Persistence;
using BS.Application.Models;
using BS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace BS.Application.Features.Queries.Transactions.Download
{
    public class DownloadStatementQueryHandler : IRequestHandler<DownloadStatementQuery, PDFReturn>
    {
        private readonly IUnitOfWork _db;
        private readonly UserManager<BankUser> _userManager;

        public DownloadStatementQueryHandler(IUnitOfWork db, UserManager<BankUser> userManager)
        {
            this._db = db;
            this._userManager = userManager;
        }
        public async Task<PDFReturn> Handle(DownloadStatementQuery request, CancellationToken cancellationToken)
        {
            var bankaccount = await _db.BankAccountRepo.GetOneAsync(x => x.BankUserId == request.userId);
            var userTransactions = await _db.TransactionRepo.GetAllAsync("SenderAccount,ReceiverAccount", x => x.ReceiverId == bankaccount.Id
                || x.SenderId == bankaccount.Id);

            var user = await _userManager.FindByIdAsync(request.userId);

            //Generate pdf with pdfCore
            var pdfResponse = GeneratePdfwithPdfCore(bankaccount, user, userTransactions);
            return pdfResponse;

        }


        private PDFReturn GeneratePdfwithPdfCore(BankAccount bankaccount, BankUser user, 
            IEnumerable<Transaction> userTransactions)
        {
            var document = new PdfDocument();

            DownloadStatementRequest statementRequest = new()
            {
                AccountNumer = bankaccount.AccountNumber,
                AccountName = user.FirstName + " " + user.LastName,
                Transactions = userTransactions
            };

            var HtmlContent = HtmlTemplate.StatementPdfTemplate(statementRequest);
            PdfGenerator.AddPdfPages(document, HtmlContent, PageSize.A4, 0);
            byte[]? response = null;

            using (var memoryStream = new MemoryStream())
            {
                document.Save(memoryStream);
                response = memoryStream.ToArray();
            }

            string filename = $"{bankaccount.AccountNumber}_Statement.pdf";

            return new PDFReturn
            {
                response = response,
                fileName = filename
            };
        }
    }
}
