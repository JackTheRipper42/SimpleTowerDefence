using System.Globalization;
using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyViewModel
    {
        public readonly NotifyingObject<string> TextProperty;
        public readonly NotifyingObject<Vector3> PositionProperty;
        public readonly NotifyingObject<Vector2> SizeProperty;

        private readonly StringFormatter _stringFormatter;

        private Vector3 _center;

        public EnemyViewModel()
        {
            TextProperty = new NotifyingObject<string>(string.Empty);
            PositionProperty = new NotifyingObject<Vector3>();
            SizeProperty = new NotifyingObject<Vector2>();

            SizeProperty.PropertyChanged += SizePropertyOnPropertyChanged;
            _stringFormatter = new StringFormatter(CultureInfo.CurrentCulture);
        }

        public void OnHealthChanged(float health, float maxHealth)
        {
            TextProperty.SetValue(FormatHealth(health, maxHealth));
        }

        public void OnCenterPositionChanged(Vector3 center)
        {
            _center = center;
            PositionProperty.SetValue(CalculatePosition(center, SizeProperty.GetValue()));
        }

        private void SizePropertyOnPropertyChanged(object sender, PropertyChangedEventArgs<Vector2> e)
        {
            PositionProperty.SetValue(CalculatePosition(_center, e.NewValue));
        }

        private string FormatHealth(float health, float maxHealth)
        {
            return string.Format(
                "{0}/{1}", 
                _stringFormatter.Format(health), 
                _stringFormatter.Format(maxHealth));
        }

        private static Vector3 CalculatePosition(Vector3 center, Vector2 size)
        {
            return new Vector3(
                center.x,
                center.y - size.y,
                center.z);
        }
    }
}
