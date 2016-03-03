﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Transform TowerContainer;
        public Transform EnemiyContainer;
        public LevelInfo[] Levels;
        public EnemyInfo[] Enemies;
        public int CurrentLevel;

        private Dictionary<EnemyId, GameObject> _enemies; 

        public void EnemyExists(Enemy enemy)
        {
            DestroyEnemy(enemy);
        }

        public void EnemyKilled(Enemy enemy)
        {
            DestroyEnemy(enemy);
        }

        public void SpawnEnemy(EnemyId id, IList<Vector3> path)
        {
            var prefab = _enemies[id];
            var obj = Instantiate(prefab);
            obj.transform.parent = EnemiyContainer.transform;
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
            _enemies = Enemies.ToDictionary(enemyInfo => enemyInfo.Id, enemyInfo => enemyInfo.Prefab);

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
