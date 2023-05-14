using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Persistence.EmailMessages
{
    public static class GeneralTemplate
    {
        public static string PrefixEmailMessage(string message)
        {
            string response = "<div style=\"width:100%;text-align:center;width:100%\">";
            response += "<h1>Welcome to Reen Bank</h1>";
            response += "<img src=\"https://res.cloudinary.com/dshlliomy/image/upload/c_limit,f_jpg,fl_lossy.any_format.preserve_transparency.progressive,h_1600,pg_1,q_auto,w_1600/undraw_Credit_card_payments_re_qboh_thn60m\">";
            response += "<h2>Thank you for banking with us!</h2>";

            response += message;

            response += "<a href=\"https://github.com/MaureenMOguche\">Visit my Github</a>";

            return response;
        }
    }
}
