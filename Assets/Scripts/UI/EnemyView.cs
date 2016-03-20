using Assets.Scripts.Binding;
using Assets.Scripts.UI.Bindings;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyView : MonoBehaviour
    {
        private readonly EnemyViewModel _viewModel;

        private Enemy _enemy;
        private TextBinding _textBinding;

        public EnemyView()
        {
            _viewModel = new EnemyViewModel();
        }

        protected virtual void Start()
        {
            _enemy = GetComponentInParent<Enemy>();
            _textBinding = GetComponentInChildren<TextBinding>();
            _textBinding.TextProperty.Bind(BindingType.OneWay, _viewModel.TextProperty);
            _textBinding.PositionProperty.Bind(BindingType.TwoWay, _viewModel.PositionProperty);
            _textBinding.SizeProperty.Bind(BindingType.OneWayToSource, _viewModel.SizeProperty);

            _enemy.HealthProperty.PropertyChanged += OnHealthChanged;
            _enemy.PositionProperty.PropertyChanged += OnPositionChanged;

            SetHealth(_enemy.Health, _enemy.MaxHealth);
            SetPosition(_enemy.Position);
        }

        protected virtual void OnDestroy()
        {
            _enemy.HealthProperty.PropertyChanged -= OnHealthChanged;
            _textBinding.TextProperty.ClearBinding();
            _textBinding.PositionProperty.ClearBinding();
            _textBinding.SizeProperty.ClearBinding();
        }

        private void OnHealthChanged(object sender, PropertyChangedEventArgs<float> e)
        {
            SetHealth(e.NewValue, _enemy.MaxHealth);
        }

        private void OnPositionChanged(object sender, PropertyChangedEventArgs<Vector3> e)
        {
            SetPosition(e.NewValue);
        }

        private void SetHealth(float health, float maxHealth)
        {
            _viewModel.OnHealthChanged(health, maxHealth);
        }

        private void SetPosition(Vector3 position)
        {
            var center = Camera.main.WorldToScreenPoint(position);
            _viewModel.OnCenterPositionChanged(center);
        }
    }
}
