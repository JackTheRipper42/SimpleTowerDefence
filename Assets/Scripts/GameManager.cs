using System.Collections;
using System.Linq;
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

        protected virtual void Start()
        {
            var level = GetComponentInChildren<Level>();
            var paths = level.GetPaths().ToList();

            foreach (var path in paths)
            {
                StartCoroutine(Spawner(path));
            }
        }

        private void CreateEnemy(Vector3[] path)
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

        private IEnumerator Spawner(Vector3[] path)
        {
            yield return new WaitForSeconds(4f);

            for (var i = 0; i < 20; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    CreateEnemy(path);
                    yield return new WaitForSeconds(0.25f);
                }
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}
