using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace NapierBankMessaging
{
    struct txtSpeakItem
    {
        private string _abv;
        private string _full;

        public string Abv { get => _abv; set => _abv = value; }
        public string Full { get => _full; set => _full = value; }

        public txtSpeakItem(string abv, string full)
        {
            _abv = abv;
            _full = full;
        }
    }

    class Database
    {
        private List<txtSpeakItem> _txtDictionary = new List<txtSpeakItem>();

        public List<txtSpeakItem> TxtDictionary
        {
            get { return _txtDictionary; }
        }

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

                _txtDictionary.Add(item);
            }
        }

        public bool WriteToJSON(object mType)
        {
            //write to JSON
            DataContractJsonSerializer js = new DataContractJsonSerializer(mType.GetType());
            MemoryStream ms = new MemoryStream();
            StreamReader sr = new StreamReader(ms);
            js.WriteObject(ms, mType);
            ms.Position = 0;
            using (StreamWriter sw = new StreamWriter("TEST.json", true))
            {
                sw.WriteLine(sr.ReadToEnd());
                ms.Close();
                sr.Close();
            }

            return true;
        }

        public string[] ReadFile(string filepath)
        {
            string[] tmp = File.ReadAllLines(filepath);

            return tmp;
        }

        public Stack<string[]> PrepareQueue(string filepath)
        {
            Stack<string[]> _messageListRAW = new Stack<string[]>();

            string[] files = Directory.GetFiles(filepath, "*.txt");

            foreach(string file in files)
            {
                string[] tmp = File.ReadAllLines(file);
                _messageListRAW.Push(tmp);
            }

            return _messageListRAW;
        }
    }
}
