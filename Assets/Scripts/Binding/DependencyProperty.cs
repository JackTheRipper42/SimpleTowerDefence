using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Assets.Scripts.Binding
{
    public sealed class DependencyProperty<T> : IDependencyProperty<T>
    {
        private const string AlreadyBoundMessage = "A binding alreay exist.";

        private readonly CoerceValueCallback<T> _coerceValueCallback;
        private readonly PropertyChangedCallback<T> _propertyChangedCallback;
        private readonly IBindingFactory _bindingFactory;

        private T _value;
        private IBinding _binding;
        private CultureInfo _culture;

        public DependencyProperty(
            [NotNull] IBindingFactory bindingFactory)
            : this(bindingFactory, default(T), null, null)
        {
        }

        public DependencyProperty(
            [NotNull] IBindingFactory bindingFactory,
            [CanBeNull] T value,
            [CanBeNull] PropertyChangedCallback<T> propertyChangedCallback,
            [CanBeNull] CoerceValueCallback<T> coerceValueCallback)
        {
            if (bindingFactory == null)
            {
                throw new ArgumentNullException("bindingFactory");
            }

            _bindingFactory = bindingFactory;
            _value = value;
            _propertyChangedCallback = propertyChangedCallback;
            _coerceValueCallback = coerceValueCallback;
        }

        public event EventHandler<PropertyChangedEventArgs<T>> PropertyChanged;

        public CultureInfo Culture
        {
            get { return _culture; }
            set
            {
                _culture = value;
                if (_binding != null)
                {
                    _binding.Culture = _culture;
                }
            }
        }

        public bool Bound { get { return _binding != null; } }

        public void SetValue(T value)
        {
            if (Equals(value, _value))
            {
                return;
            }

            var oldValue = _value;
            var newValue = _coerceValueCallback != null ? _coerceValueCallback(value) : value;
            if (Equals(newValue, _value))
            {
                return;
            }

            _value = newValue;
            OnPropertyChanged(oldValue, newValue);
        }

        public void Bind(BindingType bindingType, INotifyingObject<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (Bound)
            {
                throw new InvalidOperationException(AlreadyBoundMessage);
            }

            _binding = _bindingFactory.CreatePropertyBinding(bindingType, this, source, UnityValueConverter<T>.Instance);
            _binding.Culture = _culture;
        }

        public void Bind<TSource>(
            BindingType bindingType, 
            INotifyingObject<TSource> source,
            TwoWayValueConverter<TSource, T> converter)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            if (Bound)
            {
                throw new InvalidOperationException(AlreadyBoundMessage);
            }

            _binding = _bindingFactory.CreatePropertyBinding(bindingType, this, source, converter);
            _binding.Culture = _culture;
        }

        public void Bind<TSource>(INotifyingObject<TSource> source, OneWayValueConverter<TSource, T> converter)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            if (Bound)
            {
                throw new InvalidOperationException(AlreadyBoundMessage);
            }

            _binding = _bindingFactory.CreatePropertyBinding(BindingType.OneWay, this, source, converter);
            _binding.Culture = _culture;
        }

        public void Bind<TSource>(INotifyingObject<TSource> source, OneWayToSourceValueConverter<TSource, T> converter)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            if (Bound)
            {
                throw new InvalidOperationException(AlreadyBoundMessage);
            }

            _binding = _bindingFactory.CreatePropertyBinding(BindingType.OneWayToSource, this, source, converter);
            _binding.Culture = _culture;
        }

        public void ClearBinding()
        {
            if (_binding != null)
            {
                _binding.Close();
                _binding = null;
                SetValue(default(T));
            }
        }

        public T GetValue()
        {
            return _value;
        }

        public void Dispose()
        {
            ClearBinding();
        }

        private void OnPropertyChanged(T oldValue, T newValue)
        {
            var callback = _propertyChangedCallback;
            if (callback != null)
            {
                callback(oldValue, newValue);
            }
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(null, new PropertyChangedEventArgs<T>(oldValue, newValue));
            }
        }
    }
}