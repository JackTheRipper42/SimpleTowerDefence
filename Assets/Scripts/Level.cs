using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Level : MonoBehaviour
    {
        public Path[] Paths;

        protected virtual void Start()
        {
            var gameManager = GetComponentInParent<GameManager>();
            var enemyCount = new Dictionary<EnemyId, int>();
            foreach (var path in Paths)
            {
                path.CountEnemies(enemyCount);
            }
            gameManager.SetOverallEnemyCount(enemyCount);
            Debug.Log("enemy count:");
            foreach (var kvp in enemyCount)
            {
                Debug.Log(string.Format("type:{0} count:{1}", kvp.Key, kvp.Value));
            }
            foreach (var path in Paths)
            {
                path.SpawnEnemies();
            }
        }
    }
}
