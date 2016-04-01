using Assets.Scripts.Xml;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class RangeRenderer : MonoBehaviour
    {
        public Color Color;
        public int Points = 50;
        public float Width = 1f;
      
        public void Initialize(float radius)
        {
            var lineRenderer = GetComponent<LineRenderer>();
            var points = new Vector3[Points + 1];
            for (var i = 0; i < Points; i++)
            {
                var index = i;

                var angle = 2*Mathf.PI/Points*index;

                points[index] = CalculatePoint(radius, angle);
            }
            points[Points] = points[0];

            lineRenderer.SetColors(Color, Color);
            lineRenderer.SetVertexCount(points.Length);
            lineRenderer.SetPositions(points);
            lineRenderer.SetWidth(Width, Width);
        }

        private static Vector3 CalculatePoint(float radius, float angle)
        {
            var x = radius * Mathf.Cos(angle);
            var z = radius * Mathf.Sin(angle);
            return new Vector3(x, 0.2f, z);
        }
    }
}
