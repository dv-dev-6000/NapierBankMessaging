﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapierBankMessaging
{
    class MessageBase
    {
        // Class Variables
        public Database db = new Database();
        protected Control _con;

        protected string _rawHeader;
        protected string[] _rawBody;

        // Accessor methods
        public string MessageHeader { get => _rawHeader; set => _rawHeader = value; }
        public string[] MessageBody { get => _rawBody; set => _rawBody = value; }
        public Control Con { get => _con; }

        public MessageBase(string header, string[] body, Control con)
        {
            _rawHeader = header;
            _rawBody = body;

            _con = con;
        }

    }
}
