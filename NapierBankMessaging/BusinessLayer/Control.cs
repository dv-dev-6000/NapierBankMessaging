using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapierBankMessaging
{
    class Control
    {
        // Class Variables
        private MessageBase _rawMessage, _currMessage;

        public Control()
        {
            

            
        }

        /// <summary>
        /// Returns and integer value representing message type 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="body"></param>
        /// <returns>Int (0 == Undefined, 1 == SMS, 2 == Email, 3 == Email[SIR], 4 == Tweet)</returns>
        public int GetCurrentMessageType(MessageBase raw)
        {
            string header = raw.MessageHeader;
            string[] body = raw.MessageBody;

            int tmp = 0;

            switch (header[0])
            {
                case 'S':
                    tmp = 1;
                    break;
                case 'E':
                    if (isSIR(body))
                    {
                        tmp = 3;
                    }
                    else { tmp = 2; }
                    break;
                case 'T':
                    tmp = 4;
                    break;
                default:
                    tmp = 0;
                    break;


            }

            return tmp;
        }

        /// <summary>
        /// Checks email type messages to determine if they are SIR emails
        /// </summary>
        /// <param name="body"></param>
        /// <returns>bool (true if SIR, false if standard)</returns>
        private bool isSIR(string[] body)
        {
            bool tmp = false;

            // look at email subject for line begining "SIR"
            string subject = body[1];
            string first3 = subject.Substring(0, 3);

            // look at first line for line begining "sort code:"
            string line1 = body[2];
            string first10 = line1.Substring(0, 10);

            if (first3 == "SIR" && first10 == "Sort Code:")
            {
                tmp = true;
            }

            return tmp;
        }
    }
}
