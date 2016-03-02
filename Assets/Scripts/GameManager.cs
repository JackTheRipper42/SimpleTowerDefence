using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject EnemyPrefab;
        public Transform Turrets;
        public Transform Enemies;
        public LevelInfo[] Levels;
        public int CurrentLevel;

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

        public float GetTime()
        {
            return Time.time;
        }

        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        protected virtual void Start()
        {
            var level = Levels[CurrentLevel];
            var obj = Instantiate(level.LevelPrefab);
            obj.transform.position = new Vector3(0f, 0f, 0f);
            obj.transform.parent = transform;
            obj.transform.name = level.Name;

        }

        private void DestroyEnemy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
