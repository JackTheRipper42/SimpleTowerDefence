using JetBrains.Annotations;
using System;
using System.Globalization;

namespace Assets.Scripts.Binding
{
    public class PropertyBinding<TSource, TTarget> : IBinding
    {
        private readonly IDependencyProperty<TTarget> _target;
        private readonly INotifyingObject<TSource> _source;
        private readonly ValueConverter<TSource, TTarget> _converter; 

        public PropertyBinding(
            BindingType bindingType,
            [NotNull] IDependencyProperty<TTarget> target,
            [NotNull] INotifyingObject<TSource> source,
            [NotNull] ValueConverter<TSource,TTarget> converter)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            _target = target;
            _source = source;
            _converter = converter;

            switch (bindingType)
            {
                case BindingType.OneWay:
                    _source.PropertyChanged += SourceOnPropertyChanged;
                    SetTarget(_source.GetValue());
                    break;
                case BindingType.TwoWay:
                    _source.PropertyChanged += SourceOnPropertyChanged;
                    _target.PropertyChanged += TargetOnPropertyChanged;
                    SetTarget(_source.GetValue());
                    SetSource(_target.GetValue());
                    break;
                case BindingType.OneWayToSource:
                    _target.PropertyChanged += TargetOnPropertyChanged;
                    SetSource(_target.GetValue());
                    break;
                default:
                    throw new NotSupportedException(string.Format(
                        "The binding type '{0}' is not supported.",
                        bindingType));
            }
        }

        public CultureInfo Culture { get; set; }

        private void SourceOnPropertyChanged(object sender, PropertyChangedEventArgs<TSource> args)
        {
            SetTarget(args.NewValue);
        }

        private void TargetOnPropertyChanged(object sender, PropertyChangedEventArgs<TTarget> args)
        {
            SetSource(args.NewValue);
        }

        private void SetTarget(TSource value)
        {
            var converted = _converter.Convert(value, Culture);
            _target.SetValue(converted);
        }

        private void SetSource(TTarget value)
        {
            var converted = _converter.ConvertBack(value, Culture);
            _source.SetValue(converted);
        }

        private void UnsubscribeEventHandlers()
        {
            _source.PropertyChanged -= SourceOnPropertyChanged;
            _target.PropertyChanged -= TargetOnPropertyChanged;
        }

        protected virtual void Dispose(bool disposing)
        {
            UnsubscribeEventHandlers();
        }

        public void Close()
        {
            UnsubscribeEventHandlers();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PropertyBinding()
        {
            Dispose(false);
        }
    }
}
