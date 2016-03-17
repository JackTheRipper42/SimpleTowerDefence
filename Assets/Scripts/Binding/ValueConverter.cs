using System.Globalization;

namespace Assets.Scripts.Binding
{
    public abstract class ValueConverter<TSource,TTarget>
    {
        public abstract TTarget Convert(TSource source, CultureInfo culture);

        public abstract TSource ConvertBack(TTarget target, CultureInfo culture);
    }
}
