using System;

namespace Assets.Scripts.Binding
{
    public interface ICommand
    {
        void Execute();

        bool CanExecute();

        event EventHandler CanExecuteChanged;
    }
}
