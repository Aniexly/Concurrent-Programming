using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModel
{
    public class Command : ICommand
    {
        private readonly Action<object?> _execute;
        public Predicate<object?>? Predicate
        {
            get;
            set
            {
                field = value;
                OnCanExecuteChanged();
            }
        }

        public event EventHandler? CanExecuteChanged;

        public Command(Action<object?> execute) : this(execute, null) { }

        public Command(Action<object?> execute, Predicate<object?>? canExecute) => (_execute, Predicate) = (execute, canExecute);

        public bool CanExecute(object? parameter)
        {
            return Predicate == null || Predicate(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
