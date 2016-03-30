using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Lua;
using MoonSharp.Interpreter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Transform TowerContainer;
        public Transform EnemiyContainer;
        public Canvas Canvas;
        public LevelInfo[] Levels;
        public GameObject[] Enemies;
        public GameObject[] Towers;
        public int CurrentLevel;
        [Range(0f, 2f)] public float MaxSpawnOffset = 1f;

        private IDictionary<EnemyId, GameObject> _enemyPrefabs;
        private IDictionary<TowerId, GameObject> _towerPrefabs;
        //private IDictionary<EnemyId, int> _enemyCount;
        //private IDictionary<EnemyId, int> _remainingEnemyCount;
        //private IDictionary<EnemyId, int> _escaptedEnemyCount;
        private HashSet<Vector3> _towerPositions; 

        public void EnemyExists(Enemy enemy)
        {
            //_escaptedEnemyCount[enemy.Id] += 1;
            //_remainingEnemyCount[enemy.Id] -= 1;
            DestroyEnemy(enemy);
        }

        public void EnemyKilled(Enemy enemy)
        {
            //_remainingEnemyCount[enemy.Id] -= 1;
            DestroyEnemy(enemy);
        }

        public void SpawnEnemy(EnemyId id, IList<Vector3> path)
        {
            var prefab = _enemyPrefabs[id];
            var obj = Instantiate(prefab);
            var enemy = obj.GetComponent<Enemy>();
            obj.transform.parent = EnemiyContainer.transform;
            var offset = Random.insideUnitSphere*MaxSpawnOffset;
            offset.y = 0f;
            enemy.Position = path[0] + offset;
            enemy.SetPath(path, offset);
        }

        public void SpawnTower(TowerId id, Vector3 position)
        {
            _towerPositions.Add(position);
            var prefab = _towerPrefabs[id];
            var obj = Instantiate(prefab);
            obj.transform.parent = TowerContainer.transform;
            obj.transform.position = position;
        }

        public bool CanSpawnTower(Vector3 position)
        {
            return !_towerPositions.Contains(position);
        }

        //public void SetOverallEnemyCount(IDictionary<EnemyId, int> enemyCount)
        //{
        //    _enemyCount = enemyCount;
        //    _remainingEnemyCount = enemyCount.ToDictionary(
        //        keyValuePair => keyValuePair.Key,
        //        keyValuePair => keyValuePair.Value);
        //    _escaptedEnemyCount = enemyCount.ToDictionary(
        //        keyValuePair => keyValuePair.Key,
        //        keyValuePair => 0);
        //}

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
            _towerPositions = new HashSet<Vector3>();
            _enemyPrefabs = Enemies.ToDictionary(
                enemy => enemy.GetComponent<Enemy>().Id,
                enemy => enemy);
            _towerPrefabs = Towers.ToDictionary(
                tower => tower.GetComponent<Tower>().Id,
                tower => tower);
            //_enemyCount = new Dictionary<EnemyId, int>();
            //_remainingEnemyCount = new Dictionary<EnemyId, int>();
            //_escaptedEnemyCount = new Dictionary<EnemyId, int>();

            var levelInfo = Levels[CurrentLevel];
            LoadLevel(levelInfo);
        }

        private void DestroyEnemy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }

        private void LoadLevel(LevelInfo levelInfo)
        {
            var obj = Instantiate(levelInfo.Prefab);
            obj.transform.position = new Vector3(0f, 0f, 0f);
            obj.transform.parent = Canvas.transform;
            obj.transform.name = levelInfo.Name;

            var script = new Script(CoreModules.Preset_HardSandbox);

            UserData.RegisterType<ISpawner>();
            UserData.RegisterType<IDebugger>();

            script.Globals.Set(LuaScriptConstants.DebugGlobalName, UserData.Create(new Debugger()));

            var paths = obj.GetComponentsInChildren<Path>();
            var spawners = new List<Spawner>();
            foreach (var path in paths)
            {
                var spawner = new Spawner(path.GetPath());
                spawners.Add(spawner);
                var spawnerName = string.Format("{0}{1}", path.name, LuaScriptConstants.SpawnerGlobalNameSuffix);
                script.Globals.Set(spawnerName, UserData.Create(spawner));
            }

            var scriptPath = new FileInfo(System.IO.Path.Combine(LuaScriptConstants.LevelScriptFolder, levelInfo.Script));
            var scriptCode = File.ReadAllText(scriptPath.FullName);
            script.DoString(scriptCode);

            var setupSpawnFunction = script.Globals.Get(LuaScriptConstants.SetupWaveFunctionName);
            if (setupSpawnFunction.IsNil())
            {
                throw new InvalidOperationException(string.Format(
                    "The '{0}' function does not exist in script '{1}'.",
                    LuaScriptConstants.SetupWaveFunctionName,
                    scriptPath.Name));
            }

            script.Call(script.Globals[LuaScriptConstants.SetupWaveFunctionName], 0);

            foreach (var spawner in spawners)
            {
                StartCoroutine(SpawnCoroutine(spawner));
            }
        }

        private IEnumerator SpawnCoroutine(Spawner spawner)
        {
            foreach (var action in spawner.Actions)
            {
                var waitAction = action as WaitAction;
                if (waitAction != null)
                {
                    yield return new WaitForSeconds((float) waitAction.Time);
                }

                var spawnAction = action as SpawnAction;
                if (spawnAction != null)
                {
                    SpawnEnemy(spawnAction.Id, spawner.Path);
                }

                var debugAction = action as DebugAction;
                if (debugAction != null)
                {
                    Debug.Log(debugAction.Message);
                }
            }
        }
    }
}
