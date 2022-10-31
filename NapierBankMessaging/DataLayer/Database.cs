using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapierBankMessaging
{
    struct Incident
    {

    }

    class Database
    {
        //Lists
        List<Incident> SIRList = new List<Incident>();
        List<string> URLList = new List<string>();
        List<string> MentionList = new List<string>();
        List<string> TrendingList = new List<string>();

        public Database()
        {

        }


        private string SelectFile()
        {
            string fp = null;

            // get the filepath to load selected file

            return fp;
        }

        public void ReadFile(string filepath)
        {
            // detect type of message
            // load in info from selected file
        }


        private string SelectFolder()
        {
            string fp = null;

            // get the filepath to load selected file

            return fp;
        }

        public void PrepareQueue(string filepath)
        {
            // detect type of message
            // load in info from selected file
        }
    }
}
