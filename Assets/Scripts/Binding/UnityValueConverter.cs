using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.Binding
{
    public class UnityValueConverter<T> : ValueConverter<T,T>
    {
        private static UnityValueConverter<T> _instance;

        public static UnityValueConverter<T> Instance
        {
            get { return _instance ?? (_instance = new UnityValueConverter<T>()); }
        }

        public override T Convert(T source, CultureInfo culture)
        {
            return source;
        }

        public override T ConvertBack(T target, CultureInfo culture)
        {
            return target;
        }
    }
}
