using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows;

namespace NapierBankMessaging
{
    class Control
    {
        // Class Variables
        private SMS _sms;
        private EmailStandard _email;
        private EmailSIR _emailSIR;
        private Tweet _tweet;
        private Database db = new Database();

        // Lists
        private List<string> _SIRList = new List<string>();
        private List<string> _URLList = new List<string>();
        private List<string> _MentionList = new List<string>();
        private List<string> _TrendingList = new List<string>();
        private Stack<string[]> _messageListRAW;

        #region Accessor Methods

        public Stack<string[]> MessageList
        {
            get { return _messageListRAW; }
        }

        public List<string> TrendingList
        {
            get { return _TrendingList; }
        }

        public List<string> MentionList
        {
            get { return _MentionList; }
        }

        public List<string> URLList
        {
            get { return _URLList; }
        }

        public List<string> SIRList
        {
            get { return _SIRList; }
        }


        public SMS SMS
        {
            get
            {
                return _sms;
            }
        }

        public EmailStandard Email
        {
            get
            {
                return _email;
            }
        }

        public EmailSIR EmailSIR
        {
            get
            {
                return _emailSIR;
            }
        }

        public Tweet Tweet
        {
            get
            {
                return _tweet;
            }
        }

        public Database DB
        {
            get
            {
                return db;
            }
        }

        #endregion

        public Control()
        {
            

            
        }

        private void ShowFormatError(string text, string type)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            System.Windows.MessageBox.Show("The content does not conform to the format expected for messages of type: " + type + "\n\n" +
                                            text, "Message Format Error", button, icon);
        }

        /// <summary>
        /// Returns and integer value representing message type 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="body"></param>
        /// <returns>Int (0 == Undefined, 1 == SMS, 2 == Email, 3 == Email[SIR], 4 == Tweet)</returns>
        public int GetCurrentMessageType(MessageBase raw)
        {
            string header = raw.MessageHeader;
            string[] body = raw.MessageBody;

            int tmp = 0;

            switch (header[0])
            {
                case 'S':
                    try
                    {
                        //create object
                        _sms = new SMS(header, body, raw.Con);
                        //write to json
                        db.WriteToJSON(_sms);
                        //set return value
                        tmp = 1;
                    }
                    catch
                    {
                        ShowFormatError("Line 1 should contain a valid phone number\n" +
                            "The following lines should contain the message body (max 140 characters)", "SMS");
                    }
                    

                    break;
                case 'E':
                    if (isSIR(body))
                    {
                        try
                        {
                            //create object
                            _emailSIR = new EmailSIR(header, body, raw.Con, 4);
                            //write to json
                            db.WriteToJSON(_emailSIR);
                            //set return value
                            tmp = 3;
                        }
                        catch
                        {
                            ShowFormatError("Line 1 should contain the sender Email Address\n" +
                                "Line 2 should contain the message subject beginning SIR\n" +
                                "Line 3 should contain the branch sort code (Sort Code: 00 00 00)\n" +
                                "Line 4 should contain incident info (Nature of Incident: theft etc.)\n" +
                                "The following lines should contain the message body (max 1028 characters)", "SIR Email");
                        }
                    }
                    else 
                    {
                        try
                        {
                            //create object
                            _email = new EmailStandard(header, body, raw.Con, 2);
                            //write to json
                            db.WriteToJSON(_email);
                            //set return value
                            tmp = 2;
                        }
                        catch
                        {
                            ShowFormatError("Line 1 should contain the sender Email Address\n" +
                                "Line 2 should contain the message subject\n" +
                                "The following lines should contain the message body (max 1028 characters)", "Email");
                        }
                    }
                    break;
                case 'T':
                    try
                    {
                        //create object
                        _tweet = new Tweet(header, body, raw.Con);
                        //write to json
                        db.WriteToJSON(_tweet);
                        //set return value
                        tmp = 4;
                    }
                    catch
                    {
                        ShowFormatError("Line 1 should contain the Sender with leading @ symbol (@sender)\n" +
                            "The following lines should contain the message body (max 140 characters)", "Tweet");
                    }
                    

                    break;
                default:
                    //set return value
                    tmp = 0;
                    break;


            }

            return tmp;
        }

        /// <summary>
        /// Checks email type messages to determine if they are SIR emails
        /// </summary>
        /// <param name="body"></param>
        /// <returns>bool (true if SIR, false if standard)</returns>
        private bool isSIR(string[] body)
        {
            bool tmp= false;
            
            try
            {
                // look at email subject for line begining "SIR"
                string subject = body[1];
                string first3 = subject.Substring(0, 3);

                if (first3 == "SIR")
                {
                    tmp = true;
                }
            }
            catch
            {
                tmp = false;
            }

            return tmp;
        }

        public string[] SelectFile()
        {
            // open file explorer and get filepath
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string filePath;

                openFileDialog.InitialDirectory = "D:\\Uni\\3rd Year\\TESTING";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.Title = "Choose a file to load";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    return db.ReadFile(filePath);

                }
                else
                {
                    return null;
                }
            }
        }

        public bool SelectFolder()
        {
            //code to select target folder
            string filepath;

            FolderBrowserDialog chooseFile = new FolderBrowserDialog();
            chooseFile.SelectedPath = "D:\\Uni\\3rd Year\\TESTING";
            chooseFile.Description = "Select a folder to load files from";

            //DialogResult result = chooseFile.ShowDialog();
            if (chooseFile.ShowDialog() == DialogResult.OK)
            {
                filepath = chooseFile.SelectedPath;

                _messageListRAW = db.PrepareQueue(filepath);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
