using System.Windows;
using System.Windows.Input;

namespace FetchNotification.Commands
{
    /// <summary>
    /// Shows the main window.
    /// </summary>
    public class ExitCommand : CommandBase<ExitCommand>
    {
        public override void Execute(object parameter)
        {
            System.Windows.Application.Current.Shutdown();
            CommandManager.InvalidateRequerySuggested();
        }


        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}