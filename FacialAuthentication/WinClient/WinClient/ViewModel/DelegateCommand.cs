using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FaceAuth.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        private readonly Func<bool> _func;

        public DelegateCommand(Action action, Func<bool> func)
        {
            _action = action;

            _func = func;
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning disable 67

        public bool CanExecute(object parameter)
        {
            return _func();
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
