using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp2
{
    class RelayCommand : ICommand
    {
        private Action<object> _action;
        private Func<Boolean> _canExecute;
        public RelayCommand(Action<object> action, Func<Boolean> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        #region ICommand Members
        public bool CanExecute(object parameter)
        {
            if(_canExecute == null)
            {
                return true;
            }
            return _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                _action(parameter);
            }
            else
            {
                _action("NTR");
            }
        }
        #endregion
        /*
        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged += 1; 
        }
        */
    }
}
