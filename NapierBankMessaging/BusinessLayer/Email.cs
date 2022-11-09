using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NapierBankMessaging
{
    [DataContract]
    class EmailStandard : MessageBase
    {
        // Class Vars
        [DataMember]
        protected string TYPE = "Standard Email";
        [DataMember]
        protected string _sender;
        [DataMember]
        protected string _subject;
        [DataMember]
        protected List<string> _filteredBody;

        public string Sender { get => _sender; }
        public string Subject { get => _subject; }
        public List<string> FilteredBody { get => _filteredBody; }

        public EmailStandard(string header, string[] body, Control con, int bodStart) : base(header, body, con)
        {
            _sender = ExtractSender(_rawBody);
            _subject = ExtractSubject(_rawBody);
            _filteredBody = FilterBody(_rawBody, bodStart);
        }

        protected virtual string ExtractSender(string[] body)
        {
            string sender = body[0];
            sender = sender.TrimEnd('\r', '\n', '\t');
            return sender;
        }

        protected virtual string ExtractSubject(string[] body)
        {
            string sub = body[1];
            sub = sub.TrimEnd('\r', '\n', '\t');
            return sub;
        }

        /// <summary>
        /// look for textspeak abvs and expand out
        /// </summary>
        /// <param name="rawbody"></param>
        /// <returns>Returns a filtered string array</returns>
        protected virtual List<string> FilterBody(string[] rawbody, int bodyStart)
        {
            // var to store new filtered text
            List<string> newBod = new List<string>();

            // check for http
            // check for https
            // check for www.
            // loop through each line of the message {SKIP FIRST LINE}
            for (int j = bodyStart; j < rawbody.Length; j++)
            {
                // split current line in to a string array of individual words
                string[] words;
                words = rawbody[j].Split(' ');

                // loop through each word
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = words[i].TrimEnd('\r', '\n', '\t');

                    //check for URLs
                    if (words[i].StartsWith("http") || words[i].StartsWith("https") || words[i].StartsWith("www."))
                    {
                        //send to quarentine list
                        _con.URLList.Add(words[i]);

                        //replace with message
                        words[i] = "<URL Quarantined>";
                    }
                }

                // rebuild words into line
                string line = null;
                foreach (string word in words)
                {
                    if (line == null)
                    {
                        line = word;
                    }
                    else { line = line + " " + word; }
                }

                // add new line to newBody
                newBod.Add(line);
            }

            return newBod;
        }

    }




    [DataContract]
    class EmailSIR : EmailStandard
    {
        // Class Vars
        [DataMember]
        private string _sortCode;
        [DataMember]
        private string _IncidentNature;

        public string Sort { get => _sortCode; }
        public string Nature { get => _IncidentNature; }

        public EmailSIR(string header, string[] body, Control con, int bodStart) : base(header, body, con, bodStart)
        {
            _sortCode = ExtractSort(_rawBody);
            _IncidentNature = ExtractNature(_rawBody);

            _con.SIRList.Add(_sortCode + " | " + _IncidentNature);

            TYPE = "SIR Email";
        }

        private string ExtractSort(string[] body)
        {
            string num = body[2];

            num = num.Substring(11);
            num = num.TrimEnd('\r', '\n', '\t');

            return num;
        }

        private string ExtractNature(string[] body)
        {
            string nat = body[3];

            nat = nat.Substring(19);
            nat = nat.TrimEnd('\r', '\n', '\t');

            return nat;
        }
    }
}
