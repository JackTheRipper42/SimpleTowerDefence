using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Transform TowerContainer;
        public Transform EnemiyContainer;
        public LevelInfo[] Levels;
        public GameObject[] Enemies;
        public GameObject[] Towers;
        public int CurrentLevel;
        [Range(0f, 2f)] public float MaxSpawnOffset = 1f;

        private IDictionary<EnemyId, GameObject> _enemyPrefabs;
        private IDictionary<TowerId, GameObject> _towerPrefabs;
        private IDictionary<EnemyId, int> _enemyCount;
        private IDictionary<EnemyId, int> _remainingEnemyCount;
        private IDictionary<EnemyId, int> _escaptedEnemyCount; 

        public void EnemyExists(Enemy enemy)
        {
            _escaptedEnemyCount[enemy.Id] += 1;
            _remainingEnemyCount[enemy.Id] -= 1;
            DestroyEnemy(enemy);
        }

        public void EnemyKilled(Enemy enemy)
        {
            _remainingEnemyCount[enemy.Id] -= 1;
            DestroyEnemy(enemy);
        }

        public void SpawnEnemy(EnemyId id, IList<Vector3> path)
        {
            var prefab = _enemyPrefabs[id];
            var obj = Instantiate(prefab);
            obj.transform.parent = EnemiyContainer.transform;
            obj.transform.position = path[0];
            var enemy = obj.GetComponent<Enemy>();
            var offset = Random.insideUnitSphere*MaxSpawnOffset;
            offset.y = 0f;
            enemy.SetPath(path, offset);
        }

        public void SpawnTower(TowerId id, Vector3 position)
        {
            var prefab = _towerPrefabs[id];
            var obj = Instantiate(prefab);
            obj.transform.parent = TowerContainer.transform;
            obj.transform.position = position;
        }

        public void SetOverallEnemyCount(IDictionary<EnemyId, int> enemyCount)
        {
            _enemyCount = enemyCount;
            _remainingEnemyCount = enemyCount.ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => keyValuePair.Value);
            _escaptedEnemyCount = enemyCount.ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => 0);
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
            _enemyPrefabs = Enemies.ToDictionary(
                enemy => enemy.GetComponent<Enemy>().Id,
                enemy => enemy);
            _towerPrefabs = Towers.ToDictionary(
                tower => tower.GetComponent<Tower>().Id,
                tower => tower);
            _enemyCount = new Dictionary<EnemyId, int>();
            _remainingEnemyCount = new Dictionary<EnemyId, int>();
            _escaptedEnemyCount = new Dictionary<EnemyId, int>();

            var levelInfo = Levels[CurrentLevel];
            var obj = Instantiate(levelInfo.Prefab);
            obj.transform.position = new Vector3(0f, 0f, 0f);
            obj.transform.parent = transform;
            obj.transform.name = levelInfo.Name;
            var preBuildTowers = obj.GetComponentsInChildren<Tower>();
            foreach (var turret in preBuildTowers)
            {
                turret.transform.parent = TowerContainer;
            }
        }

        private void DestroyEnemy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
