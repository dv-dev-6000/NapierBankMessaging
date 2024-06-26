﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace NapierBankMessaging
{
    [DataContract]
    class SMS : MessageBase
    {
        // Class Vars
        [DataMember]
        private string _phoneNumber;
        [DataMember]
        private List<string> _filteredBody;

        public string PhoneNumber { get => _phoneNumber; }
        public List<string> FilteredBody { get => _filteredBody;}


        public SMS(string header, string[] body, Control con) : base(header, body, con)
        {
            _phoneNumber = ExtractNumber(_rawBody);

            _filteredBody = FilterBody(_rawBody);

            TYPE = "SMS";
        }

        private string ExtractNumber(string[] body)
        {
            string num = body[0];
            num = num.TrimEnd('\r', '\n', '\t');
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
                    words[i] = words[i].TrimEnd('\r', '\n', '\t');

                    // expand text speak words if found
                    foreach (txtSpeakItem item in _con.DB.TxtDictionary)
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
                                switch (words[i][words[i].Length-1])
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
}
