using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class TowerView<TTower, TModel, TLevel> : MonoBehaviour
        where TTower : Tower<TModel, TLevel>
        where TModel : TowerModel<TLevel>
        where TLevel : TowerLevel
    {
        protected readonly TowerViewModel ViewModel;

        private TModel _towerModel;
        private RangeRenderer _rangeRenderer;
        private SpriteRendererBinding _towerRendererBinding;

        protected TowerView()
        {
            ViewModel = new TowerViewModel();
        }

        protected virtual void Start()
        {
            var tower = GetComponent<TTower>();
            _towerModel = tower.Model;
            _towerModel.LevelProperty.PropertyChanged += LevelOnPropertyChanged;

            ViewModel.OnLevelChange(_towerModel.Levels[_towerModel.LevelProperty.GetValue()]);

            _rangeRenderer = GetComponentInChildren<RangeRenderer>();
            _rangeRenderer.VisibleProperty.Bind(BindingType.OneWay, ViewModel.VisibleProperty);

            _towerRendererBinding = tower.TowerTransform.gameObject.GetComponent<SpriteRendererBinding>();
            _towerRendererBinding.SpriteProperty.Bind(BindingType.OneWay, ViewModel.TowerSpriteProperty);
        }

        protected virtual void OnMouseEnter()
        {
            ViewModel.OnMouseEnter();
        }

        protected virtual void OnMouseExit()
        {
            ViewModel.OnMouseExit();
        }

        protected virtual void OnDestroy()
        {
            if (_rangeRenderer != null)
            {
                _rangeRenderer.VisibleProperty.ClearBinding();
                _rangeRenderer = null;
            }
            if (_towerRendererBinding != null)
            {
                _towerRendererBinding.SpriteProperty.ClearBinding();
                _towerRendererBinding = null;
            }
        }

        private void LevelOnPropertyChanged(object sender, PropertyChangedEventArgs<int> e)
        {
            var level = _towerModel.Levels[e.NewValue];
            ViewModel.OnLevelChange(level);
        }
    }
}
