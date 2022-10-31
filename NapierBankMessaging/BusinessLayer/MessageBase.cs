using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapierBankMessaging
{
    class MessageBase
    {
        // Class Variables
        protected string _messageHeader;
        protected string _messageBody;

        // Accessor methods
        public string MessageHeader { get => _messageHeader; set => _messageHeader = value; }
        public string MessageBody { get => _messageBody; set => _messageBody = value; }

        public MessageBase(string header, string body)
        {
            _messageHeader = header;
            _messageBody = body;
        }

        
    }
}
