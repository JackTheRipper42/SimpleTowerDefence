using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EnemyViewModel
    {
        public readonly NotifyingObject<string> TextProperty = new NotifyingObject<string>(string.Empty);
        public readonly NotifyingObject<Vector3> PositionProperty = new NotifyingObject<Vector3>();
        public readonly NotifyingObject<Vector2> SizeProperty = new NotifyingObject<Vector2>();

        private Vector3 _center;

        public EnemyViewModel()
        {
            SizeProperty.PropertyChanged += SizePropertyOnPropertyChanged;
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

        private static string FormatHealth(float health, float maxHealth)
        {
            return string.Format("{0}/{1}", health.ToString("F"), maxHealth.ToString("F"));
        }

        private static Vector3 CalculatePosition(Vector3 center, Vector2 size)
        {
            //return new Vector3(
            //    center.x - size.x/2,
            //    center.y - size.y,
            //    center.z);
            return new Vector3(
                center.x,
                center.y - size.y,
                center.z);
        }
    }
}
