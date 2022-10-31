using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapierBankMessaging
{
    class EmailStandard : MessageBase
    {
        // Class Vars
        protected string sender;
        protected string subject;
        protected string filteredBody;

        public EmailStandard(string header, string[] body) : base(header, body)
        {

        }
    }

    class EmailSIR : EmailStandard
    {
        // Class Vars
        private string sortCode;
        private string IncidentNature;

        public EmailSIR(string header, string[] body) : base(header, body)
        {

        }
    }
}
