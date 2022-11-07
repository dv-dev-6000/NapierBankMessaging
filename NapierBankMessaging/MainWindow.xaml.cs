﻿using System;
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
                        tBox_SMSBody.AppendText(con.SMS.FilteredBody[i] + "\b");
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
                        tBox__emailBody.AppendText(con.Email.FilteredBody[i] + "\b");
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
                        tBox_eBodySIR.AppendText(con.EmailSIR.FilteredBody[i] + "\b");
                    }
                    break;
                case 4:
                    tBox_tweetSender.Clear();
                    tBox_tweetBody.Clear();
                    // Display SMS
                    tBox_tweetSender.Text = con.Tweet.Sender;
                    for (int i = 0; i < con.Tweet.FilteredBody.Count; i++)
                    {
                        tBox_tweetBody.AppendText(con.Tweet.FilteredBody[i] + "\b");
                    }

                    break;
                default:
                    // show error / default message
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string header = tBox_NewMsgHeader.Text;
            string[] body = new string[tBox_NewMsgBody.LineCount];

            for (int i = 0; i < tBox_NewMsgBody.LineCount; i++)
            {
                body[i] = tBox_NewMsgBody.GetLineText(i);
            } 
            
            MessageBase test = new MessageBase(header, body, con);

            ProcessMessage(test);
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
    }
}
