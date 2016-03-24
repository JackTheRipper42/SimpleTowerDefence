using JetBrains.Annotations;
using System;
using System.Globalization;

namespace Assets.Scripts
{
    public class StringFormatter
    {
        private readonly CultureInfo _culture;
        private readonly char _decimalSeparator;

        public StringFormatter([NotNull] CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }

            _culture = culture;
            _decimalSeparator = _culture.NumberFormat.NumberDecimalSeparator[0];
        }

        public string Format(float value)
        {
            var str = value.ToString("F", _culture);
            var parts = str.Split(_decimalSeparator);
            var integralPart = parts[0];
            if (parts.Length > 1)
            {
                var fractionPart = parts[1];
                fractionPart = fractionPart.TrimEnd('0');
                if (fractionPart.Length > 0)
                {
                    return string.Format("{0}{1}{2}", integralPart, _decimalSeparator, fractionPart);
                }
            }
            return integralPart;
        }
    }
}
