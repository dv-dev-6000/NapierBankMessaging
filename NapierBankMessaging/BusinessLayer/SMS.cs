using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapierBankMessaging
{
    class SMS : MessageBase
    {
        // Class Vars
        private string phoneNumber;
        private string filteredBody;

        public SMS(string header, string[] body) : base(header, body)
        {

        }
    }
}
