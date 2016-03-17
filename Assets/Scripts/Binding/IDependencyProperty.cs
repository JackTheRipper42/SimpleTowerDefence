using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Assets.Scripts.Binding
{
    public interface IDependencyProperty<T> : IDisposable
    {
        event EventHandler<PropertyChangedEventArgs<T>> PropertyChanged;

        T GetValue();

        void SetValue(T value);

        void Bind(
            BindingType bindingType, 
            [NotNull] INotifyingObject<T> source);

        void Bind<TSource>(
            BindingType bindingType,
            [NotNull] INotifyingObject<TSource> source,
            [NotNull] TwoWayValueConverter<TSource, T> converter);

        void Bind<TSource>(
            [NotNull] INotifyingObject<TSource> source,
            [NotNull] OneWayValueConverter<TSource, T> converter);

        void Bind<TSource>(
            [NotNull] INotifyingObject<TSource> source,
            [NotNull] OneWayToSourceValueConverter<TSource, T> converter);

        void ClearBinding();

        CultureInfo Culture { get; set; }

        bool Bound { get; }
    }
}