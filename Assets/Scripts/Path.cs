using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Path : MonoBehaviour
    {
        public abstract Vector3[] GetPath();
    }
}
