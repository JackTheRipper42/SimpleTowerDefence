using System;
using System.Globalization;

namespace Assets.Scripts.Binding
{
    public abstract class OneWayValueConverter<TSource, TTarget> : ValueConverter<TSource, TTarget>
    {
        public override TSource ConvertBack(TTarget source, CultureInfo culture)
        {
            throw new NotSupportedException(string.Format(
                "The '{0}' convert does not support conversion back to source.",
                GetType().Name));
        }
    }
}
