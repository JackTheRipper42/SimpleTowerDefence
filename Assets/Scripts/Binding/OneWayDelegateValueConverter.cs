using JetBrains.Annotations;
using System;
using System.Globalization;

namespace Assets.Scripts.Binding
{
    public class OneWayDelegateValueConverter<TSource, TTarget> : OneWayValueConverter<TSource, TTarget>
    {
        private readonly Func<TSource, CultureInfo, TTarget> _convert;


        public OneWayDelegateValueConverter([NotNull] Func<TSource, CultureInfo, TTarget> convert)
        {
            if (convert == null)
            {
                throw new ArgumentNullException("convert");
            }

            _convert = convert;
        }

        public override TTarget Convert(TSource source, CultureInfo culture)
        {
            return _convert(source, culture);
        }
    }
}
