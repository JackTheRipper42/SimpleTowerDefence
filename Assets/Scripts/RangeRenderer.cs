using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class RangeRenderer : MonoBehaviour
    {
        public readonly DependencyProperty<bool> VisibleProperty; 

        public Color Color;
        public int Points = 50;
        public float Width = 1f;

        private LineRenderer _lineRenderer;

        public RangeRenderer()
        {
            VisibleProperty = new DependencyProperty<bool>(
                BindingFactory.Instance,
                false,
                VisiblePropertyChangedCallback,
                null);
        }

        public void Initialize(float radius)
        {
            _lineRenderer = GetComponent<LineRenderer>();
            var points = new Vector3[Points + 1];
            for (var i = 0; i < Points; i++)
            {
                var index = i;

                var angle = 2*Mathf.PI/Points*index;

                points[index] = CalculatePoint(radius, angle);
            }
            points[Points] = points[0];

            _lineRenderer.SetColors(Color, Color);
            _lineRenderer.SetVertexCount(points.Length);
            _lineRenderer.SetPositions(points);
            _lineRenderer.SetWidth(Width, Width);
            _lineRenderer.enabled = false;
        }

        private static Vector3 CalculatePoint(float radius, float angle)
        {
            var x = radius * Mathf.Cos(angle);
            var z = radius * Mathf.Sin(angle);
            return new Vector3(x, 0.2f, z);
        }

        private void VisiblePropertyChangedCallback(bool oldValue, bool newValue)
        {
            _lineRenderer.enabled = newValue;
        }

    }
}
