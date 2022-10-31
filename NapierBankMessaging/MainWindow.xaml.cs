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

            //string[] dictionary = System.IO.File.ReadAllLines(@"DataLayer/textwords.csv");

        }

        private void HideMessageGrids()
        {
            grd_SMSDisplay.Visibility = Visibility.Hidden;
            grd_TweetDisplay.Visibility = Visibility.Hidden;
            grd_SIREmailDisplay.Visibility = Visibility.Hidden;
            grd_EmailDisplay.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string header = tBox_NewMsgHeader.Text;
            MessageBase test = new MessageBase(header, new string[] { "Hello", "SIRwqadw", "Sort Code:awd" });

            switch (con.GetCurrentMessageType(test))
            {
                case 0:
                    HideMessageGrids();
                    // show error / default message
                    break;
                case 1:
                    HideMessageGrids();
                    grd_SMSDisplay.Visibility = Visibility.Visible;
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
    }
}
