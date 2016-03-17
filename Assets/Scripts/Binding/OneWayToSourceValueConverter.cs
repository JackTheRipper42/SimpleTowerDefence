using System;
using System.Globalization;

namespace Assets.Scripts.Binding
{
    public abstract class OneWayToSourceValueConverter<TSource,TTarget> : ValueConverter<TSource, TTarget>
    {
        public override TTarget Convert(TSource source, CultureInfo culture)
        {
            throw new NotSupportedException(string.Format(
                "The '{0}' convert does not support conversion from source.",
                GetType().Name));
        }
    }
}
