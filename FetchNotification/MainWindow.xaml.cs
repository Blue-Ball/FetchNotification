using Hardcodet.Wpf.TaskbarNotification;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NotificationManager _notificationManager = new NotificationManager();
        public static MainWindow mainWnd = null;
        public static SettingWindow settingWnd = null;
        public static SettingWindowConnected settingWndConnected = null;
        static WatsonWsClient _Client = null;

        public MainWindow()
        {
            InitializeComponent();

            mainWnd = this;
        }

        static void InitializeClient(String strServerProtocal, String strServerAddress, int nServerPort, string strParam)
        {
            // For Test
            strServerProtocal = "ws";
            strServerAddress = "localhost";
            nServerPort = 9006;
            strParam = "123";

            String strURI = strServerProtocal + "://" + strServerAddress + ":" + nServerPort.ToString() + "/" + strParam;
            InitializeClientURI(strURI);
        }

        static void InitializeClientURI(String NewURI)
        {
            if (_Client != null)
                _Client.Dispose();
            _Client = null;

            // original constructor
            //_Client = new WatsonWsClient(_ServerIp, _ServerPort, _Ssl);
            _Client = new WatsonWsClient(new Uri(NewURI));


            _Client.ServerConnected += ServerConnected;
            _Client.ServerDisconnected += ServerDisconnected;
            _Client.MessageReceived += MessageReceived;
            _Client.Logger = Logger;

            // await _Client.StartAsync();
            _Client.StartAsync();
            //_Client.StartWithTimeout(10);

            //_Client.StartWithTimeoutAsync();
            if (!_Client.Connected)
            {
                mainWnd.Dispatcher.Invoke(delegate {
                    mainWnd.StatusChange(Properties.Resources.connection_fail, NotificationType.Error);
                });
            }
        }

        static void ServerConnected(object sender, EventArgs args)
        {
            mainWnd.Dispatcher.Invoke(delegate {
                mainWnd.StatusChange(Properties.Resources.connected, NotificationType.Success);
            });
        }

        static void ServerDisconnected(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Server disconnected");
            mainWnd.Dispatcher.Invoke(delegate {
                mainWnd.StatusChange(Properties.Resources.connection_fail, NotificationType.Error);
            });
        }

        static void Logger(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void StatusChange(String msg, NotificationType type)
        {
            var content = new NotificationContent
            {
                Title = "Server Status",
                Message = msg,
                Type = type
            };

            BalloonIcon bi = type == NotificationType.Error ? BalloonIcon.Error : BalloonIcon.Info;
            tb.ShowBalloonTip("Server Status", msg, bi);
        }

        public void ReceivedMessage(String msg)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            //String strResponse = strResponse = "{ 'con':'" + DateTime.Now.Ticks.ToString() + "','type':'lead','image':'https://lh5.googleusercontent.com/-HJDyncz7x1A/AAAAAAAAAAI/AAAAAAAD5LQ/G2hYxZn_pDg/photo.jpg?sz=64','message':'Hello Today','link':'html/body/div[3]/div[1]/div/div/h1/img'}"; ;
            dynamic msgJson = JsonConvert.DeserializeObject(msg);
            var content = new NotificationContent
            {
                Title = msgJson.con,
                Message = msgJson.message,
                Type = NotificationType.Information,
                msg_type = msgJson.type,
                image = msgJson.image,
                link = msgJson.link,
                time = DateTime.Now,
                time_view = DateTime.Now.ToString("hh:mm tt")
            };

            _notificationManager.Show(content, "WindowArea", TimeSpan.FromMilliseconds(-1),
                onClick: () => OpenUrl(content));

            bool add_lead_notification = bool.Parse(data["NOTIFICATIONS"]["add_lead"]);
            bool call_bounce_notification = bool.Parse(data["NOTIFICATIONS"]["call_bounce"]);
            bool schedualed_mission_notification = bool.Parse(data["NOTIFICATIONS"]["schedualed_mission"]);
            bool new_mission_notification = bool.Parse(data["NOTIFICATIONS"]["new_mission"]);
            bool new_email_notification = bool.Parse(data["NOTIFICATIONS"]["new_email"]);
            bool chat_message_notification = bool.Parse(data["NOTIFICATIONS"]["chat_message"]);
            bool service_message_notification = bool.Parse(data["NOTIFICATIONS"]["service_message"]);
            bool mission_message_notification = bool.Parse(data["NOTIFICATIONS"]["mission_message"]);
            bool system_message_notification = bool.Parse(data["NOTIFICATIONS"]["system_message"]);

            bool add_lead_mute = bool.Parse(data["SOUNDS"]["add_lead"]);
            bool call_bounce_mute = bool.Parse(data["SOUNDS"]["call_bounce"]);
            bool schedualed_mission_mute = bool.Parse(data["SOUNDS"]["schedualed_mission"]);
            bool new_mission_mute = bool.Parse(data["SOUNDS"]["new_mission"]);
            bool new_email_mute = bool.Parse(data["SOUNDS"]["new_email"]);
            bool chat_message_mute = bool.Parse(data["SOUNDS"]["chat_message"]);
            bool service_message_mute = bool.Parse(data["SOUNDS"]["service_message"]);
            bool mission_message_mute = bool.Parse(data["SOUNDS"]["mission_message"]);
            bool system_message_mute = bool.Parse(data["SOUNDS"]["system_message"]);

            if (msgJson.type == "add_lead")
            {
                if (add_lead_notification)
                    _notificationManager.Show(content, "", null, onClick: () => OpenUrl(content));
                if (add_lead_mute)
                {
                    SoundPlayer player = new SoundPlayer("notification.wav");
                    player.Load();
                    player.Play();
                }
            }

            if (msgJson.type == "call_bounce")
            {
                if (add_lead_notification)
                    _notificationManager.Show(content, "", null, onClick: () => OpenUrl(content));
                if (add_lead_mute)
                {
                    SoundPlayer player = new SoundPlayer("notification.wav");
                    player.Load();
                    player.Play();
                }
            }

            if (msgJson.type == "schedualed_mission")
            {
                if (add_lead_notification)
                    _notificationManager.Show(content, "", null, onClick: () => OpenUrl(content));
                if (add_lead_mute)
                {
                    SoundPlayer player = new SoundPlayer("notification.wav");
                    player.Load();
                    player.Play();
                }
            }

            if (msgJson.type == "new_mission")
            {
                if (add_lead_notification)
                    _notificationManager.Show(content, "", null, onClick: () => OpenUrl(content));
                if (add_lead_mute)
                {
                    SoundPlayer player = new SoundPlayer("notification.wav");
                    player.Load();
                    player.Play();
                }
            }

            if (msgJson.type == "new_email")
            {
                if (add_lead_notification)
                    _notificationManager.Show(content, "", null, onClick: () => OpenUrl(content));
                if (add_lead_mute)
                {
                    SoundPlayer player = new SoundPlayer("notification.wav");
                    player.Load();
                    player.Play();
                }
            }

            if (msgJson.type == "chat_message")
            {
                if (add_lead_notification)
                    _notificationManager.Show(content, "", null, onClick: () => OpenUrl(content));
                if (add_lead_mute)
                {
                    SoundPlayer player = new SoundPlayer("notification.wav");
                    player.Load();
                    player.Play();
                }
            }

            if (msgJson.type == "service_message")
            {
                if (add_lead_notification)
                    _notificationManager.Show(content, "", null, onClick: () => OpenUrl(content));
                if (add_lead_mute)
                {
                    SoundPlayer player = new SoundPlayer("notification.wav");
                    player.Load();
                    player.Play();
                }
            }

            if (msgJson.type == "mission_message")
            {
                if (add_lead_notification)
                    _notificationManager.Show(content, "", null, onClick: () => OpenUrl(content));
                if (add_lead_mute)
                {
                    SoundPlayer player = new SoundPlayer("notification.wav");
                    player.Load();
                    player.Play();
                }
            }

            if (msgJson.type == "system_message")
            {
                if (add_lead_notification)
                    _notificationManager.Show(content, "", null, onClick: () => OpenUrl(content));
                if (add_lead_mute)
                {
                    SoundPlayer player = new SoundPlayer("notification.wav");
                    player.Load();
                    player.Play();
                }
            }
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            string msg = "(null)";
            if (args.Data != null && args.Data.Length > 0)
                msg = Encoding.UTF8.GetString(args.Data);
            System.Diagnostics.Debug.WriteLine(args.MessageType.ToString() + " from server: " + msg);

            mainWnd.Dispatcher.Invoke(delegate {
                mainWnd.ReceivedMessage(msg);
            });
        }

        private void OpenUrl(NotificationContent content)
        {
            string url = content.link;
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseDown += delegate { DragMove(); };

            connectToServer();
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            if(_Client != null && _Client.Connected)
            {
                if(MainWindow.settingWndConnected == null)
                    MainWindow.settingWndConnected = new SettingWindowConnected();
                MainWindow.settingWndConnected.Show();
            }
            else
            {
                if (MainWindow.settingWnd == null)
                    MainWindow.settingWnd = new SettingWindow();
                MainWindow.settingWnd.Show();
            }

            this.Hide();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            
        }

        public void connectToServer()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Configuration.ini");

            string strServerProtocal = data["CONNECTION"]["SERVER_PROTOCAL"];
            string strServerAddress = data["CONNECTION"]["SERVER_ADDRESS"];
            int nServerPort = int.Parse(data["CONNECTION"]["SERVER_PORT"]);
            string strParam = "";
            if (data["CONNECTION"]["USER_ID"].Length > 0)
                strParam = "?id=" + data["CONNECTION"]["USER_ID"];

            InitializeClient(strServerProtocal, strServerAddress, nServerPort, strParam);
        }

        public void DisconnectToServer()
        {
            if (_Client != null)
                _Client.Dispose();
            _Client = null;
        }

        private void btnAlart_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
