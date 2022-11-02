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
        private string _phoneNumber;
        private List<string> _filteredBody;

        public string PhoneNumber { get => _phoneNumber; }
        public List<string> FilteredBody { get => _filteredBody;}


        public SMS(string header, string[] body) : base(header, body)
        {
            _phoneNumber = ExtractNumber(_rawBody);

            _filteredBody = FilterBody(_rawBody);

            
        }

        private string ExtractNumber(string[] body)
        {
            string num = "TEST";

            return num;
        }

        /// <summary>
        /// look for textspeak abvs and expand out
        /// </summary>
        /// <param name="rawbody"></param>
        /// <returns>Returns a filtered string array</returns>
        private List<string> FilterBody(string[] rawbody)
        {
            // var to store new filtered text
            List<string> newBod = new List<string>();

            // loop through each line of the message
            for (int j = 0; j < rawbody.Length; j++)
            {
                // split current line in to a string array of individual words
                string[] words;
                words = rawbody[j].Split(' ');

                // loop through each word
                for (int i = 0; i < words.Length; i++) 
                {
                    // expand text speak words if found
                    foreach (txtSpeakItem item in db.txtDictionary)
                    {
                        // Find lone textspeak words
                        if (item.abv == words[i])
                        {
                            words[i] = words[i] + " <" + item.full + ">";
                            break;
                        }

                        // Find textspeak words with punctuation
                        if (words[i].EndsWith(".") || words[i].EndsWith(",") || words[i].EndsWith(":") || words[i].EndsWith(";"))
                        {
                            string newWord = words[i].Remove(words[i].Length - 1, 1);

                            if (item.abv == newWord)
                            {
                                switch (words[i][words[i].Length-1])
                                {
                                    case '.':
                                        words[i] = newWord + " <" + item.full + ">.";
                                        break;
                                    case ',':
                                        words[i] = newWord + " <" + item.full + ">,";
                                        break;
                                    case ':':
                                        words[i] = newWord + " <" + item.full + ">:";
                                        break;
                                    case ';':
                                        words[i] = newWord + " <" + item.full + ">;";
                                        break;
                                }
                                
                                break;
                            }
                        }
                    }
                }

                // rebuild words into line
                string line = null;
                foreach (string word in words)
                {
                    line = line + " " + word;
                }

                // add new line to newBody
                newBod.Add(line);
            }

            return newBod;
        }
    }
}
