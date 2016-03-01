using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject EnemyPrefab;
        public Transform Turrets;
        public Transform Enemies;

        public void EnemyExists(Enemy enemy)
        {
            DestroyEnemy(enemy);
        }

        public void EnemyKilled(Enemy enemy)
        {
            DestroyEnemy(enemy);
        }

        public void SpawnEnemy(IList<Vector3> path)
        {
            var obj = Instantiate(EnemyPrefab);
            obj.transform.parent = Enemies.transform;
            obj.transform.position = path[0];
            var enemy = obj.GetComponent<Enemy>();
            enemy.SetPath(path);
        }

        private void DestroyEnemy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
