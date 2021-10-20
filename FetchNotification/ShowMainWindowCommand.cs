using System.Windows;
using System.Windows.Input;

namespace FetchNotification.Commands
{
    /// <summary>
    /// Shows the main window.
    /// </summary>
    public class ShowMainWindowCommand : CommandBase<ShowMainWindowCommand>
    {
        public override void Execute(object parameter)
        {
            if (MainWindow.settingWnd != null)
                MainWindow.settingWnd.Hide();
            Window win = GetTaskbarWindow(parameter);
            if (win.IsVisible)
                win.Hide();
            else
                win.Show();
            CommandManager.InvalidateRequerySuggested();
        }


        public override bool CanExecute(object parameter)
        {
            Window win = GetTaskbarWindow(parameter);
            //return win != null && !win.IsVisible;
            return win != null;
        }
    }
}