using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string WorkPath = "work";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            var directory = new DirectoryInfo(WorkPath);
            var files = directory.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                string destination = file.FullName.Replace(directory.FullName + @"\", "");
                if(destination == "Updater" || destination == "Updater.dll" || 
                    destination == "Configuration.ini")
                {

                }
                else
                {
                    Directory.CreateDirectory(new FileInfo(destination).DirectoryName);
                    file.CopyTo(destination, true);
                }
            }

            // Clean up.
            Directory.Delete(WorkPath, true);

            // Restart.
            //Process thisprocess = Process.GetCurrentProcess();
            //thisprocess.CloseMainWindow();
            //thisprocess.Close();
            //thisprocess.Dispose();
            string mainProgram = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "FetchNotification.exe");
            var spawn = Process.Start(mainProgram);

            this.Close();
        }
    }
}
