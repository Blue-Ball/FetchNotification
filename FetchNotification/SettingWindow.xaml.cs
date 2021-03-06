using Hardcodet.Wpf.TaskbarNotification;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using WatsonWebsocket;

namespace FetchNotification
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseDown += delegate { DragMove(); };

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            txtUserID.Text = data["CONNECTION"]["USER_ID"];

            txtUserID.Focus();
            txtUserID.CaretIndex = txtUserID.Text.Length;
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            string strServerProtocal = data["CONNECTION"]["SERVER_PROTOCAL"];
            string strServerAddress = data["CONNECTION"]["SERVER_ADDRESS"];
            int nServerPort = int.Parse(data["CONNECTION"]["SERVER_PORT"]);
            string strParam = "";
            if (data["CONNECTION"]["USER_ID"].Length > 0)
                strParam = "?id=" + data["CONNECTION"]["USER_ID"];
            if(txtUserID.Text.Length > 0)
            {
                data["CONNECTION"]["USER_ID"] = txtUserID.Text;
                parser.WriteFile("Configuration.ini", data);
            }

            this.Hide();
            MainWindow.mainWnd.Show();
            MainWindow.mainWnd.ConnectToServer();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow.mainWnd.Show();
        }
    }
}
