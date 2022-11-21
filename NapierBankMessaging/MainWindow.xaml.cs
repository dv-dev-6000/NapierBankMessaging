using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace NapierBankMessaging
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Control con = new Control();

        public MainWindow()
        {
            InitializeComponent();
            HideMessageGrids();

        }

        private void HideMessageGrids()
        {
            grd_SMSDisplay.Visibility = Visibility.Hidden;
            grd_TweetDisplay.Visibility = Visibility.Hidden;
            grd_SIREmailDisplay.Visibility = Visibility.Hidden;
            grd_EmailDisplay.Visibility = Visibility.Hidden;
        }

        private void ClearMessageWriter()
        {
            // Clear Old
            tBox_NewMsgHeader.Clear();
            tBox_NewMsgBody.Clear();
        }

        private void ProcessMessage(MessageBase raw)
        {
            switch (con.GetCurrentMessageType(raw))
            {
                case 0:
                    HideMessageGrids();
                    // show error / default message
                    break;
                case 1:
                    HideMessageGrids();
                    grd_SMSDisplay.Visibility = Visibility.Visible;
                    DisplayMessage(1);
                    break;
                case 2:
                    HideMessageGrids();
                    grd_EmailDisplay.Visibility = Visibility.Visible;
                    DisplayMessage(2);
                    break;
                case 3:
                    HideMessageGrids();
                    grd_SIREmailDisplay.Visibility = Visibility.Visible;
                    DisplayMessage(3);
                    break;
                case 4:
                    HideMessageGrids();
                    grd_TweetDisplay.Visibility = Visibility.Visible;
                    DisplayMessage(4);
                    break;
                default:
                    HideMessageGrids();
                    // show error / default message
                    break;
            }
        }

        public void DisplayMessage(int type)
        {
            ClearMessageWriter();

            switch (type)
            {
                case 1:
                    tBox_SMSBody.Clear();
                    tBox_SMSNumber.Clear();
                    // Display SMS
                    tBox_SMSNumber.Text = con.SMS.PhoneNumber;
                    for (int i = 0; i < con.SMS.FilteredBody.Count; i++)
                    {
                        tBox_SMSBody.AppendText(con.SMS.FilteredBody[i] + "\n");
                    }
                    break;
                case 2:
                    tBox_emailSender.Clear();
                    tBox_emailSubject.Clear();
                    tBox__emailBody.Clear();
                    // Display Email
                    tBox_emailSender.Text = con.Email.Sender;
                    tBox_emailSubject.Text = con.Email.Subject;
                    for (int i = 0; i < con.Email.FilteredBody.Count; i++)
                    {
                        tBox__emailBody.AppendText(con.Email.FilteredBody[i] + "\n");
                    }
                    break;
                case 3:
                    tBox_eBodySIR.Clear();
                    tBox_eSenderSIR.Clear();
                    tBox_eSubjectSIR.Clear();
                    tBox_eSortSIR.Clear();
                    tBox_eNatureSIR.Clear();
                    // Display Email
                    tBox_eSenderSIR.Text = con.EmailSIR.Sender;
                    tBox_eSubjectSIR.Text = con.EmailSIR.Subject;
                    tBox_eSortSIR.Text = con.EmailSIR.Sort;
                    tBox_eNatureSIR.Text = con.EmailSIR.Nature;
                    for (int i = 0; i < con.EmailSIR.FilteredBody.Count; i++)
                    {
                        tBox_eBodySIR.AppendText(con.EmailSIR.FilteredBody[i] + "\n");
                    }
                    break;
                case 4:
                    tBox_tweetSender.Clear();
                    tBox_tweetBody.Clear();
                    // Display SMS
                    tBox_tweetSender.Text = con.Tweet.Sender;
                    for (int i = 0; i < con.Tweet.FilteredBody.Count; i++)
                    {
                        tBox_tweetBody.AppendText(con.Tweet.FilteredBody[i] + "\n");
                    }
                    break;
                default:
                    // show error / default message
                    break;
            }
        }

        private void ShowWarning(string text)
        {
            string messageBoxText = text;
            string caption = "Input Error!";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;

            System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // check text boxes have content
            if (tBox_NewMsgHeader.Text.Length > 0 && tBox_NewMsgBody.Text.Length > 0)
            {
                // pass content to variables
                string header = tBox_NewMsgHeader.Text;
                string[] body = new string[tBox_NewMsgBody.LineCount];
                for (int i = 0; i < tBox_NewMsgBody.LineCount; i++)
                {
                    body[i] = tBox_NewMsgBody.GetLineText(i);
                }

                // check header has correct number of chars
                if (header.Length == 10)
                {
                    // check header has correct leading char
                    if (header.StartsWith('S') || header.StartsWith('T') || header.StartsWith('E'))
                    {
                        bool proceed = false;
                        try
                        {
                            string tmp = header.Substring(1);
                            int.Parse(tmp);
                            proceed = true;
                        }
                        catch
                        {
                            // show warning
                            ShowWarning("Message Header should begin 'S', 'E' or 'T' followed by 9 NUMBERS");
                        }

                        if (proceed)
                        {
                            // create message object instance and process message
                            MessageBase test = new MessageBase(header, body, con);
                            ProcessMessage(test);
                        }
                    }
                    else
                    {
                        // show warning
                        ShowWarning("Message Header should begin with capital letter 'S', 'E' or 'T'");
                    }
                }
                else
                {
                    // show warning
                    ShowWarning("Message Header should have a total character length of 10. ('S', 'E' or 'T' followed by 9 digits)");
                }
            }
            else
            {
                // show warning
                ShowWarning("Both 'Message Header' & 'Message Body' text boxs should contain text.");
            }

            LoadNext();
        }

        private void btn_MentionList_Click(object sender, RoutedEventArgs e)
        {
            ListView listWindow = new ListView();
            listWindow.lstBx_List.ItemsSource = con.MentionList;
            listWindow.lbl_title.Content = "Mention List";
            listWindow.Show();
        }

        private void btn_TrendingList_Click(object sender, RoutedEventArgs e)
        {
            List<string> uniqueOccurences = new List<string>();
            List<string> displayList = new List<string>();

            // create list of unique trending items
            foreach (string item in con.TrendingList)
            {
                if (!uniqueOccurences.Contains(item))
                {
                    uniqueOccurences.Add(item);
                }
            }

            // cylce through unique list
            for (int i = 0; i < uniqueOccurences.Count; i++)
            {
                // count occurences of unique hashtags
                int occurenceCount = 0;

                foreach (string item in con.TrendingList)
                {
                    if (uniqueOccurences[i] == item)
                    {
                        occurenceCount++;
                    }
                }

                // concatinate data for display
                displayList.Add(occurenceCount + " " + uniqueOccurences[i]);
            }

            displayList.Sort();
            displayList.Reverse();

            ListView listWindow = new ListView();
            listWindow.lstBx_List.ItemsSource = displayList;
            listWindow.lbl_title.Content = "Trending List";
            listWindow.Show();
        }

        private void btn_quarList_Click(object sender, RoutedEventArgs e)
        {
            ListView listWindow = new ListView();
            listWindow.lstBx_List.ItemsSource = con.URLList;
            listWindow.lbl_title.Content = "Quarentine List";
            listWindow.Show();
        }

        private void btn_SIRList_Click(object sender, RoutedEventArgs e)
        {
            ListView listWindow = new ListView();
            listWindow.lstBx_List.ItemsSource = con.SIRList;
            listWindow.lbl_title.Content = "Incident List";
            listWindow.Show();
        }

        private void btn_loadSingle_Click(object sender, RoutedEventArgs e)
        {
            string[] txt = con.SelectFile();

            if (txt != null)
            {
                for (int i = 0; i < txt.Length; i++)
                {
                    if (i == 0)
                    {
                        tBox_NewMsgHeader.Text = txt[i];
                    }
                    else
                    {
                        tBox_NewMsgBody.Text = tBox_NewMsgBody.Text + txt[i] + "\n";
                    }
                }
            }
            
        }

        private void btn_loadMultiple_Click(object sender, RoutedEventArgs e)
        {
            con.SelectFolder();

            LoadNext();
        }

        private void LoadNext()
        {
            if (con.MessageList != null && con.MessageList.Count > 0)
            {
                string[] txt = con.MessageList.Pop();

                for (int i = 0; i < txt.Length; i++)
                {
                    if (i == 0)
                    {
                        tBox_NewMsgHeader.Text = txt[i];
                    }
                    else
                    {
                        tBox_NewMsgBody.Text = tBox_NewMsgBody.Text + txt[i] + "\n";
                    }
                }
            }
        }
    }
}
