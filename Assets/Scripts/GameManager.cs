using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject ExitPrefab;
        public GameObject EnemyPrefab;

        public void EnemyExists(Enemy enemy)
        {
            DestroyEnemy(enemy);
        }

        protected virtual void Start()
        {
            var level = GetComponentInChildren<Level>();
            var paths = level.GetPaths().ToList();

            foreach (var path in paths)
            {
                var exit = Instantiate(ExitPrefab);
                exit.transform.position = path.Last();
                exit.transform.parent = level.Exits.transform;

                StartCoroutine(Spawner(path, level));
            }
        }

        private void CreateEnemy(Vector3[] path, Level level)
        {
            var obj = Instantiate(EnemyPrefab);
            obj.transform.parent = level.Enemies.transform;
            obj.transform.position = path[0];
            var enemy = obj.GetComponent<Enemy>();
            enemy.SetPath(path);
        }

        private void DestroyEnemy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }

        private IEnumerator Spawner(Vector3[] path, Level level)
        {
            yield return new WaitForSeconds(4f);

            for (var i = 0; i < 20; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    CreateEnemy(path, level);
                    yield return new WaitForSeconds(0.25f);
                }
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}
