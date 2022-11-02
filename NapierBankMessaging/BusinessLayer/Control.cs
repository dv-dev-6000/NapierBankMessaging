using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapierBankMessaging
{
    enum CurrMessageType
    {
        undefined,
        sms,
        email,
        emailSIR,
        tweet
    }

    class Control
    {
        // Class Variables
        private MessageBase _rawMessage;
        private CurrMessageType _currMessType = CurrMessageType.undefined;

        private SMS _sms;
        private EmailStandard _email;
        private EmailSIR _emailSIR;
        private Tweet _tweet;

        public SMS SMS
        {
            get
            {
                return _sms;
            }
        }

        public EmailStandard Email
        {
            get
            {
                return _email;
            }
        }

        public EmailSIR EmailSIR
        {
            get
            {
                return _emailSIR;
            }
        }

        public Tweet Tweet
        {
            get
            {
                return _tweet;
            }
        }

        public CurrMessageType CurrMessageType
        {
            get
            {
                return _currMessType;
            }
        }

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
                    //set return value
                    tmp = 1;
                    //create object
                    _sms = new SMS(header, body);
                    break;
                case 'E':
                    if (isSIR(body))
                    {
                        //set return value
                        tmp = 3;
                        //create object
                        _emailSIR = new EmailSIR(header, body);
                    }
                    else 
                    {
                        //set return value
                        tmp = 2;
                        //create object
                        _email = new EmailStandard(header, body);
                    }
                    break;
                case 'T':
                    //set return value
                    tmp = 4;
                    //create object
                    _tweet = new Tweet(header, body);
                    break;
                default:
                    //set return value
                    tmp = 0;
                    break;


            }

            _currMessType = (CurrMessageType)tmp;

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
