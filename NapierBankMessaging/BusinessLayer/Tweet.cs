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
        private string _sender;
        private List<string> _filteredBody;

        public string Sender { get => _sender; }
        public List<string> FilteredBody { get => _filteredBody; }

        public Tweet(string header, string[] body, Control con) : base(header, body, con)
        {
            _sender = ExtractUserName(_rawBody);
            _filteredBody = FilterBody(_rawBody);
        }

        private string ExtractUserName(string[] body)
        {
            string num = body[0];

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

            // loop through each line of the message {SKIP FIRST LINE}
            for (int j = 1; j < rawbody.Length; j++)
            {
                // split current line in to a string array of individual words
                string[] words;
                words = rawbody[j].Split(' ');

                // loop through each word
                for (int i = 0; i < words.Length; i++)
                {
                    // expand text speak words if found
                    foreach (txtSpeakItem item in db.TxtDictionary)
                    {
                        // Find lone textspeak words
                        if (item.Abv == words[i])
                        {
                            words[i] = words[i] + " <" + item.Full + ">";
                            break;
                        }

                        // Find textspeak words with punctuation
                        if (words[i].EndsWith(".") || words[i].EndsWith(",") || words[i].EndsWith(":") || words[i].EndsWith(";"))
                        {
                            string newWord = words[i].Remove(words[i].Length - 1, 1);

                            if (item.Abv == newWord)
                            {
                                switch (words[i][words[i].Length - 1])
                                {
                                    case '.':
                                        words[i] = newWord + " <" + item.Full + ">.";
                                        break;
                                    case ',':
                                        words[i] = newWord + " <" + item.Full + ">,";
                                        break;
                                    case ':':
                                        words[i] = newWord + " <" + item.Full + ">:";
                                        break;
                                    case ';':
                                        words[i] = newWord + " <" + item.Full + ">;";
                                        break;
                                }
                                break;
                            }
                        }
                    }

                    
                    if (words[i].Length > 0)
                    {
                        // check for # or @
                        if (words[i][0] == '@')
                        {
                            // add only one instance of a name to mention list
                            if (!_con.MentionList.Contains(words[i]))
                            {
                                _con.MentionList.Add(words[i]);
                            }
                        }
                        else if (words[i][0] == '#')
                        {
                            // add every instance to trending list
                            _con.TrendingList.Add(words[i]);
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
