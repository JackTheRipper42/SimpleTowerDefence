using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Level : MonoBehaviour
    {
        public Path[] Paths;
        public GameObject Turrets;
        public GameObject Enemies;

        public IEnumerable<Vector3[]> GetPaths()
        {
            foreach (var path in Paths)
            {
                yield return path.GetPath();
                path.gameObject.SetActive(false);
            }
        }
    }
}
