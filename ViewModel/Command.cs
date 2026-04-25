using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModel
{
    public class Command : ICommand
    {
        private Action<object?> execute;
        private Predicate<object>? predicate;
        public Predicate<object>? Predicate
        {
            get => predicate;
            set
            {
                predicate = value;
                OnCanExecuteChanged();
            }
        }

        public event EventHandler? CanExecuteChanged;

        public Command(Action<object?> execute) => (this.execute, this.Predicate) = (execute, null);

        public Command(Action<object?> execute, Predicate<object> canExecute) => (this.execute, this.Predicate) = (execute, canExecute);

        public bool CanExecute(object? parameter)
        {
            return Predicate == null || Predicate(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
