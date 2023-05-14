using BS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application
{
    public static class HtmlTemplate
    {
        public static string StatementPdfTemplate(DownloadStatementRequest request)
        {
            string imageurl = "https://res.cloudinary.com/dshlliomy/image/upload/c_limit,f_jpg,fl_lossy.any_format.preserve_transparency.progressive,h_1600,pg_1,q_auto,w_1600/bsv4i0qrpfdp0hvgbtu1";
           
            //var response = "";

            var response = "<div style=\"width:100%;height:100%;\">" +
                "<div style=\"display:flex;justify-content: space-between;align-items: center;\">" +
                "<img src='" + imageurl + "' style=\"height: 100px;\">" +
                "<div style=\"width: 40%;height: 50px;background-color: #ef5b24;\"></div>" +
                "</div>" +
                "<h1 style=\"text-align: center;font-size: 3rem;\">Statement of Account</h1>" +
                "<div style=\"display: flex;justify-content:space-between;\">" +
                $"<h1>{request.AccountNumer}</h1>" +
                $"<h1>{request.AccountName}</h1>" +
                "</div>" +
                "<div style=\"width: 100%;height: 8px;background-color: #1686cb;margin-bottom:20px;\"></div>" +
                "<table style=\"width: 100%;text-align:center;\">" +
                "<thead>" +
                "<tr><th style=\"width:14%\">Date</th><th style=\"width:18%\">Debit</th><th style=\"width:18%\">Credit</th><th style=\"width:30%\">From/To</th><th style=\"width:20%\">Balance</th></tr>" +
                "</thead>" +
                "<tbody>";

            foreach (var transaction in request.Transactions)
            {

                response += "<tr>" +
                $"<td>{transaction.DateTime.ToShortDateString()}</td>";

                if (transaction.SenderAccount.AccountNumber == request.AccountNumer)
                {
                    response += $"<td style=\"color:red;font-weight:bold;\">{transaction.Amount}</td>";
                }
                else
                {
                    response += $"<td> </td>";
                }

                if (transaction.ReceiverAccount.AccountNumber == request.AccountNumer)
                {
                    response += $"<td style=\"color:green;font-weight:bold;\">{transaction.Amount}</td>";
                }
                else
                {
                    response += $"<td> </td>";
                }

                if (transaction.ReceiverAccount.AccountNumber == request.AccountNumer)
                {
                    response += $"<td>{transaction.SenderAccount.AccountNumber}</td>";
                }
                else
                {
                    response += $"<td>{transaction.ReceiverAccount.AccountNumber} </td>";
                }


                if (transaction.ReceiverAccount.AccountNumber == request.AccountNumer)
                {
                    response += $"<td>{transaction.ReceiverBalance}</td>";
                }
                else
                {
                    response += $"<td>{transaction.SenderBalance} </td>";
                }
                
                response +=
                "</tr>" +
                "<div style=\"width:100%; height:2px; background-color:#1686cb;padding-top:5px;padding-bottom:5px;\"></div>";
                

            }

            response +=
                "</tbody>" +
                "</table>" +
                "</div>";

            return response;
        }
    }
}
