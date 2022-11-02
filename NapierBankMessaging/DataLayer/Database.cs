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

    struct txtSpeakItem
    {
        public string abv;
        public string full;

        public txtSpeakItem(string abv, string full)
        {
            this.abv = abv;
            this.full = full;
        }
    }

    class Database
    {
        //Lists
        List<Incident> SIRList = new List<Incident>();
        List<string> URLList = new List<string>();
        List<string> MentionList = new List<string>();
        List<string> TrendingList = new List<string>();

        public List<txtSpeakItem> txtDictionary = new List<txtSpeakItem>();

        public Database()
        {
            FillTxtDictionary();

        }

        private void FillTxtDictionary()
        {
            string[] tmp = System.IO.File.ReadAllLines(@"DataLayer/textwords.csv");

            foreach (string line in tmp)
            {
                string[] splitline = line.Split(',');

                txtSpeakItem item = new txtSpeakItem(splitline[0], splitline[1]);

                txtDictionary.Add(item);
            }
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
