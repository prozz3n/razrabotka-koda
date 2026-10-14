using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute;
        private bool _isRunning;

        public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => !_isRunning && (_canExecute?.Invoke() ?? true);

        public async void Execute(object? parameter)
        {
            if (_isRunning || !CanExecute(parameter))
            {
                return;
            }

            _isRunning = true;
            CommandManager.InvalidateRequerySuggested();

            try
            {
                await _execute();
            }
            finally
            {
                _isRunning = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
