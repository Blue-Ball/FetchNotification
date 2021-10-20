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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NotificationManager _notificationManager = new NotificationManager();
        public static MainWindow mainWnd = null;
        static WatsonWsClient _Client = null;

        public MainWindow()
        {
            InitializeComponent();

            mainWnd = this;
        }

        static void InitializeClient(String strServerProtocal, String strServerAddress, int nServerPort, string strParam)
        {
            String strURI = strServerProtocal + "://" + strServerAddress + ":" + nServerPort.ToString() + "/" + strParam;
            InitializeClientURI(strURI);
        }

        static void InitializeClientURI(String NewURI)
        {
            if (_Client != null) _Client.Dispose();

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
                    mainWnd.StatusChange("Server Connection failed", NotificationType.Error);
                });
            }
        }

        static void ServerConnected(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Server connected");
            mainWnd.Dispatcher.Invoke(delegate {
                mainWnd.StatusChange("Server Connected", NotificationType.Success);
            });
        }

        static void ServerDisconnected(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Server disconnected");
            mainWnd.Dispatcher.Invoke(delegate {
                mainWnd.StatusChange("Server Disconnected", NotificationType.Error);
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
                //onClick: () => _notificationManager.Show(content));
                onClick: () => OpenUrl(content));

            if(this.IsVisible)
            {

            }
            else
            {
                _notificationManager.Show(content);       // for bubble notification on the tray area.
            }
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            string msg = "(null)";
            if (args.Data != null && args.Data.Length > 0) msg = Encoding.UTF8.GetString(args.Data);
            System.Diagnostics.Debug.WriteLine(args.MessageType.ToString() + " from server: " + msg);

            mainWnd.Dispatcher.Invoke(delegate {
                mainWnd.ReceivedMessage(msg);
            });
        }

        private void OpenUrl(NotificationContent content)
        {
            string url = content.link;
            return;
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
    }
}
