using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Vector3[] Path;

        protected virtual void Start()
        {
            var enemies = GetComponentsInChildren<Enemy>();
            foreach (var enemy in enemies)
            {
                enemy.SetPath(Path);
            }
        }
    }
}
