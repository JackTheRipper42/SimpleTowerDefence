using JetBrains.Annotations;
using System;

namespace Assets.Scripts.Binding
{
    public class PropertyChangedEventArgs<T> : EventArgs
    {
        private readonly T _oldValue;
        private readonly T _newValue;

        public PropertyChangedEventArgs([CanBeNull] T oldValue, [CanBeNull] T newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public T OldValue
        {
            get { return _oldValue; }
        }

        public T NewValue
        {
            get { return _newValue; }
        }
    }
}
