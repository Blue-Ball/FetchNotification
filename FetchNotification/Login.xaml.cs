using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using RedCell.Diagnostics.Update;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FetchNotification
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            // make the request
            var url = string.Format("https://biz1.co.il/appApi.php?api=DesktopLogin&username={0}&password={1}", txtUserID.Text, txtUserPassword.Password);
            var task = HTTPClientWrapper.Get(url);
            await task.ContinueWith((x) => {
                dynamic msgJson = JsonConvert.DeserializeObject(x.Result);
                string strID = msgJson.id;
                string strName = msgJson.name;
                string strToken = msgJson.token;

                Application.Current.Dispatcher.Invoke((Action)delegate {
                    if (strToken != null && strToken != "")
                    {
                        var parser = new FileIniDataParser();
                        IniData data = parser.ReadFile("Configuration.ini");

                        if (txtUserID.Text.Length > 0)
                        {
                            data["CONNECTION"]["USER_ID"] = strToken;
                            parser.WriteFile("Configuration.ini", data);
                        }

                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        // your code
                        MessageBox.Show("Login Failed.");
                    }
                });
            });
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            CommandManager.InvalidateRequerySuggested();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserID.Focus();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            //if(e.Key == Key.Escape)
            //{
            //    btnCancel_Click(null, null);
            //}
            //else if(e.Key == Key.Enter)
            //{
            //    btnConnect_Click(null, null);
            //}
            if(e.Key == Key.Enter)
            {
                btnConnect_Click(null, null);
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            var updater = new Updater();
            if (updater.IsThereUpdate())
            {
                MessageBoxResult dialogResult = MessageBox.Show("There is new Version. Would you update it?", "Update", MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.OK)
                {
                    updater.Update();
                    string updaterApp = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Updater");
                    var spawn = Process.Start(updaterApp);

                    this.Close();
                }
            }
            txtUserID.Focus();
        }
    }
}
