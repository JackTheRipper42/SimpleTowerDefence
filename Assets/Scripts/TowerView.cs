using Assets.Scripts.Binding;
using Assets.Scripts.Xml;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(ITower))]
    public class TowerView : MonoBehaviour
    {
        private readonly TowerViewModel _model;

        public TowerView()
        {
            _model = new TowerViewModel();
        }

        protected virtual void Start()
        {
            var rangeRenderer = GetComponentInChildren<RangeRenderer>();
            rangeRenderer.VisibleProperty.Bind(BindingType.OneWay, _model.VisibleProperty);
        }

        protected virtual void OnMouseEnter()
        {
            _model.OnMouseEnter();
        }

        protected virtual void OnMouseExit()
        {
            _model.OnMouseExit();
        }
    }
}
