using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Online_Chat.Command
{
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> execute;
        //private readonly Func<Task> executeTask;

        private readonly Func<bool> canExecute;
        public RelayCommand(Func<Task> execute) : this(execute, canExecute: null) // Sometimes you don't need canExecute method. So you can create a command without it like; SomeCommand = new RelayCommand(UpdateName);
        {
        }

        public RelayCommand(Func<Task> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }


        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute();
        }

        public void Execute(object parameter)
        {
            this.execute();
        }
    }
}
