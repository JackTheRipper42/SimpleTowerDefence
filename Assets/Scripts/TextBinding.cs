using Assets.Scripts.Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(ContentSizeRefitter))]
    public class TextBinding : MonoBehaviour
    {
        public readonly DependencyProperty<string> TextProperty;
        public readonly DependencyProperty<Vector3> PositionProperty;
        public readonly DependencyProperty<Vector2> SizeProperty; 

        private Text _text;
        private ContentSizeRefitter _contentSizeRefitter;

        public TextBinding()
        {
            TextProperty = new DependencyProperty<string>(
                BindingFactory.Instance,
                string.Empty,
                TextPropertyChangedCallback, 
                null);
            PositionProperty = new DependencyProperty<Vector3>(
                BindingFactory.Instance,
                new Vector3(0f,0f,0f),
                PositionPropertyChangedCallback, 
                null);
            SizeProperty = new DependencyProperty<Vector2>(
                BindingFactory.Instance,
                new Vector2(),
                SizePropertyChangedCallback,
                null);
        }

        private string Text
        {
            get { return _text.text; }
            set
            {
                _text.text = value;
                _contentSizeRefitter.Refit();
            }
        }

        private Vector3 LocalPosition
        {
            get
            {
                var rectTransform = (RectTransform) _text.transform;
                return rectTransform.localPosition;
            }
            set
            {
                var rectTransform = (RectTransform) _text.transform;
                rectTransform.localPosition = value;
            }
        }

        private Vector2 DeltaSize
        {
            get
            {
                var rectTransform = (RectTransform) _text.transform;
                return rectTransform.sizeDelta;
            }
            set
            {
                var rectTransform = (RectTransform) _text.transform;
                rectTransform.sizeDelta = value;
            }
        }

        private bool BindingActive
        {
            get
            {
                return gameObject.activeInHierarchy &&
                       _text != null &&
                       _contentSizeRefitter != null;
            }
        }

        protected virtual void Start()
        {
            _contentSizeRefitter = GetComponent<ContentSizeRefitter>();
            _text = GetComponent<Text>();
            Text = TextProperty.GetValue();
            LocalPosition = PositionProperty.GetValue();
            SizeProperty.SetValue(DeltaSize);
        }

        protected virtual void Update()
        {
            TextProperty.SetValue(Text);
            PositionProperty.SetValue(LocalPosition);
            SizeProperty.SetValue(DeltaSize);
        }

        protected virtual void OnDestroy()
        {
            _text = null;
        }

        private void TextPropertyChangedCallback(string oldValue, string newValue)
        {
            if (BindingActive)
            {
                Text = newValue;
            }
        }

        private void PositionPropertyChangedCallback(Vector3 oldValue, Vector3 newValue)
        {
            if (BindingActive)
            {
                LocalPosition = newValue;
            }
        }

        private void SizePropertyChangedCallback(Vector2 oldValue, Vector2 newValue)
        {
            if (BindingActive)
            {
                DeltaSize = newValue;
            }
        }
    }
}
