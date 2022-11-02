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
                    break;
                case 3:
                    HideMessageGrids();
                    grd_SIREmailDisplay.Visibility = Visibility.Visible;
                    break;
                case 4:
                    HideMessageGrids();
                    grd_TweetDisplay.Visibility = Visibility.Visible;
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

                    break;
                case 3:
                    
                    break;
                case 4:
                    
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
            
            MessageBase test = new MessageBase(header, body);

            ProcessMessage(test);
        }
    }
}
