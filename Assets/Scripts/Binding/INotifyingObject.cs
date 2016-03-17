using System;

namespace Assets.Scripts.Binding
{
    public interface INotifyingObject<T>
    {
        event EventHandler<PropertyChangedEventArgs<T>> PropertyChanged;
        void SetValue(T value);
        T GetValue();
    }
}