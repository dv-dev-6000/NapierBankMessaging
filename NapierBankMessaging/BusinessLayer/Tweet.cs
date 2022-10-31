using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapierBankMessaging
{
    class Tweet : MessageBase
    {
        // Class Vars
        private string sender;
        private string filteredBody;

        public Tweet(string header, string[] body) : base(header, body)
        {

        }
    }
}
