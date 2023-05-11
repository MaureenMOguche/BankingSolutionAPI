using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Constants
{
    public class TransferDepositMessages
    {
        public const string Success = "Successful";
        public const string ErrorInvalidAmount = "Invalid amount";
        public const string ErrorInvalidSenderAccount = "Invalid Sender Account Number";
        public const string ErrorInvalidReceiverAccount = "Invalid Receiver Account Number";
        public const string ErrorInsufficientBalance = "Insufficient balance";
        public const string UserNotExist = "User does not exist";

    }
}
