using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererBinding : MonoBehaviour
    {
        public readonly DependencyProperty<Sprite> SpriteProperty;

        private SpriteRenderer _spriteRenderer;

        public SpriteRendererBinding()
        {
            SpriteProperty = new DependencyProperty<Sprite>(
                BindingFactory.Instance,
                null,
                SpritePropertyChangedCallback,
                null);
        }

        protected virtual void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = SpriteProperty.GetValue();
        }

        protected virtual void OnDestroy()
        {
            _spriteRenderer = null;
        }

        private void SpritePropertyChangedCallback(Sprite oldValue, Sprite newValue)
        {
            if (BindingActive)
            {
                _spriteRenderer.sprite = newValue;
            }
        }

        private bool BindingActive
        {
            get
            {
                return gameObject.activeInHierarchy &&
                       _spriteRenderer != null;
            }
        }
    }
}
