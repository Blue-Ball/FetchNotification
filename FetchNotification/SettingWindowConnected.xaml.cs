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
    /// Interaction logic for SettingWindowConnected.xaml
    /// </summary>
    public partial class SettingWindowConnected : Window
    {
        public SettingWindowConnected()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseDown += delegate { DragMove(); };

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            txtUserID.Text = data["CONNECTION"]["USER_ID"];

            bool add_lead_notification = !bool.Parse(data["NOTIFICATIONS"]["add_lead"]);
            bool call_bounce_notification = !bool.Parse(data["NOTIFICATIONS"]["call_bounce"]);
            bool schedualed_mission_notification = !bool.Parse(data["NOTIFICATIONS"]["schedualed_mission"]);
            bool new_mission_notification = !bool.Parse(data["NOTIFICATIONS"]["new_mission"]);
            bool new_email_notification = !bool.Parse(data["NOTIFICATIONS"]["new_email"]);
            bool chat_message_notification = !bool.Parse(data["NOTIFICATIONS"]["chat_message"]);
            bool service_message_notification = !bool.Parse(data["NOTIFICATIONS"]["service_message"]);
            bool mission_message_notification = !bool.Parse(data["NOTIFICATIONS"]["mission_message"]);
            bool system_message_notification = !bool.Parse(data["NOTIFICATIONS"]["system_message"]);

            bool add_lead_mute = !bool.Parse(data["SOUNDS"]["add_lead"]);
            bool call_bounce_mute = !bool.Parse(data["SOUNDS"]["call_bounce"]);
            bool schedualed_mission_mute = !bool.Parse(data["SOUNDS"]["schedualed_mission"]);
            bool new_mission_mute = !bool.Parse(data["SOUNDS"]["new_mission"]);
            bool new_email_mute = !bool.Parse(data["SOUNDS"]["new_email"]);
            bool chat_message_mute = !bool.Parse(data["SOUNDS"]["chat_message"]);
            bool service_message_mute = !bool.Parse(data["SOUNDS"]["service_message"]);
            bool mission_message_mute = !bool.Parse(data["SOUNDS"]["mission_message"]);
            bool system_message_mute = !bool.Parse(data["SOUNDS"]["system_message"]);

            chk_add_lead_notification.IsChecked = add_lead_notification;
            chk_call_bounce_notification.IsChecked = call_bounce_notification;
            chk_schedualed_mission_notification.IsChecked = schedualed_mission_notification;
            chk_new_mission_notification.IsChecked = new_mission_notification;
            chk_new_email_notification.IsChecked = new_email_notification;
            chk_chat_message_notification.IsChecked = chat_message_notification;
            chk_service_message_notification.IsChecked = service_message_notification;
            chk_mission_message_notification.IsChecked = mission_message_notification;
            chk_system_message_notification.IsChecked = system_message_notification;

            chk_add_lead_mute.IsChecked = add_lead_mute;
            chk_call_bounce_mute.IsChecked = call_bounce_mute;
            chk_schedualed_mission_mute.IsChecked = schedualed_mission_mute;
            chk_new_mission_mute.IsChecked = new_mission_mute;
            chk_new_email_mute.IsChecked = new_email_mute;
            chk_chat_message_mute.IsChecked = chat_message_mute;
            chk_service_message_mute.IsChecked = service_message_mute;
            chk_mission_message_mute.IsChecked = mission_message_mute;
            chk_system_message_mute.IsChecked = system_message_mute;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow.mainWnd.DisconnectToServer();
            MainWindow.mainWnd.Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow.mainWnd.Show();
        }


        private void chk_add_lead_notification_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["NOTIFICATIONS"]["add_lead"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_add_lead_mute_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["SOUNDS"]["add_lead"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_call_bounce_notification_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["NOTIFICATIONS"]["call_bounce"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_call_bounce_mute_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["SOUNDS"]["call_bounce"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_schedualed_mission_notification_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["NOTIFICATIONS"]["schedualed_mission"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_schedualed_mission_mute_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["SOUNDS"]["schedualed_mission"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_new_mission_notification_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["NOTIFICATIONS"]["new_mission"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_new_mission_mute_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["SOUNDS"]["new_mission"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_new_email_notification_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["NOTIFICATIONS"]["new_email"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_new_email_mute_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["SOUNDS"]["new_email"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_chat_message_notification_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["NOTIFICATIONS"]["chat_message"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_chat_message_mute_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["SOUNDS"]["chat_message"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_service_message_notification_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["NOTIFICATIONS"]["service_message"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_service_message_mute_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["SOUNDS"]["service_message"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_mission_message_notification_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["NOTIFICATIONS"]["mission_message"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_mission_message_mute_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["SOUNDS"]["mission_message"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_system_message_notification_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["NOTIFICATIONS"]["system_message"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }

        private void chk_system_message_mute_Checked(object sender, RoutedEventArgs e)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            data["SOUNDS"]["system_message"] = (!((CheckBox)sender).IsChecked).ToString();
            parser.WriteFile("Configuration.ini", data);
        }
    }
}
