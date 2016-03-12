using UnityEngine;

namespace Assets.Scripts
{
    public class Rasterizer
    {
        public Vector3 Rasterize(Vector3 poistion)
        {
            return new Vector3(
                Rasterize(poistion.x),
                0,
                Rasterize(poistion.z));
        }

        private static float Rasterize(float value)
        {
            return Mathf.Floor(value) + 0.5f;
        }
    }
}
