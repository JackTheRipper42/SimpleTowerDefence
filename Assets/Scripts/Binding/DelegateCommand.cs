using JetBrains.Annotations;
using System;

namespace Assets.Scripts.Binding
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(
            [NotNull] Action execute,
            [NotNull] Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand([NotNull] Action execute)
            : this(execute, () => true)
        {
        }

        public void Execute()
        {
            _execute();
        }

        public bool CanExecute()
        {
            return _canExecute();
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
