﻿using Assets.Scripts.Lua;
using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Assets.Scripts.Xml;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Transform TowerContainer;
        public Transform EnemyContainer;
        public Canvas Canvas;
        public RuntimeAnimatorController[] AnimatorControllers;
        public Sprite[] Sprites;
        public GameObject EnemyPrefab;
        public GameObject DirectFireTowerPrefab;
        public GameObject AreaOfEffectTowerPrefab;
        [Range(0f, 2f)] public float MaxSpawnOffset = 1f;

        private IDictionary<string, RuntimeAnimatorController> _animatorControllers;
        private IDictionary<string, Sprite> _sprites; 
        private IDictionary<EnemyId, EnemyInfo> _enemyInfos;
        private IDictionary<TowerId, TowerInfo> _towerInfos;
        private HashSet<Enemy> _enemies; 
        private HashSet<Vector3> _towerPositions;
        private Level _level;
        
        public void EnemyExits([NotNull] Enemy enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException("enemy");
            }

            DestroyEnemy(enemy);
        }

        public void EnemyKilled([NotNull] Enemy enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException("enemy");
            }

            DestroyEnemy(enemy);
        }

        public void SpawnTower(TowerId id, Vector3 position)
        {
            _towerPositions.Add(position);
            var towerInfo = _towerInfos[id];
            var baseSprite = _sprites[towerInfo.BaseSprite];
            var towerSprite = _sprites[towerInfo.TowerSprite];
            GameObject obj;
            var infoType = towerInfo.GetType();

            if (infoType == typeof(DirectFireTowerInfo))
            {
                obj = Instantiate(DirectFireTowerPrefab);
                InitializeTower<DirectFireTowerInfo, DirectFireTower>(
                    obj,
                    (DirectFireTowerInfo) towerInfo,
                    baseSprite,
                    towerSprite);
            }
            else if (infoType == typeof(AreaOfEffectTowerInfo))
            {
                obj = Instantiate(AreaOfEffectTowerPrefab);
                InitializeTower<AreaOfEffectTowerInfo, AreaOfEffectTower>(
                    obj,
                    (AreaOfEffectTowerInfo) towerInfo,
                    baseSprite,
                    towerSprite);
            }
            else
            {
                throw new NotSupportedException(string.Format(
                    "The tower type '{0}' is not supported.",
                    towerInfo.GetType()));
            }
            obj.transform.parent = TowerContainer.transform;
            obj.transform.position = position;
        }

        public bool CanSpawnTower(Vector3 position)
        {
            return !_towerPositions.Contains(position);
        }

        public float GetTime()
        {
            return Time.time;
        }

        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        public void LoadLevel(Level level)
        {
            _level = level;
            StartCoroutine(LoadLevelCoroutine(level));
        }

        public void UnloadLevel()
        {
            if (_level == null)
            {
                return;
            }

            StopAllCoroutines();
            DestroyChildren(EnemyContainer);
            DestroyChildren(TowerContainer);
            SceneManager.UnloadScene(_level.SceneName);
            _towerPositions.Clear();
            _level = null;
        }

        public IEnumerable<Enemy> GetEnemies()
        {
            return _enemies;
        }

        protected virtual void Start()
        {
            _enemies = new HashSet<Enemy>();
            _towerPositions = new HashSet<Vector3>();
            _animatorControllers = AnimatorControllers.ToDictionary(
                controller => controller.name,
                controller => controller);
            _sprites = Sprites.ToDictionary(
                sprite => sprite.name,
                sprite => sprite);
            _enemyInfos = ParseEnemies().ToDictionary(
                enemy => enemy.Id,
                enemy => enemy);
            _towerInfos = ParseTowers().ToDictionary(
                tower => tower.Id,
                tower => tower);

            var levels = ParseLevels();
            LoadLevel(levels.First());
        }

        private static void InitializeTower<TInfo, TTower>(
            GameObject obj,
            TInfo towerInfo,
            Sprite baseSprite,
            Sprite towerSprite)
            where TInfo : TowerInfo
            where TTower : Tower<TInfo>
        {
            var tower = obj.GetComponent<TTower>();
            tower.Initialize(towerInfo, baseSprite, towerSprite);
        }

        private static void DestroyChildren(Transform container)
        {
            var count = container.childCount;
            for (var index = 0; index < count; index++)
            {
                var child = container.GetChild(index);
                Destroy(child.gameObject);
            }
        }

        private void SpawnEnemy(EnemyId id, IList<Vector3> path)
        {
            var obj = Instantiate(EnemyPrefab);
            var enemy = obj.GetComponent<Enemy>();
            obj.transform.parent = EnemyContainer.transform;
            var offset = Random.insideUnitSphere * MaxSpawnOffset;
            offset.y = 0f;
            enemy.Position = path[0] + offset;
            var enemyInfo = _enemyInfos[id];
            var animationController = _animatorControllers[enemyInfo.AnimationController];
            enemy.Initialize(
                enemyInfo,
                animationController,
                path,
                offset);
            _enemies.Add(enemy);
        }

        private void DestroyEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }

        private static IEnumerable<Level> ParseLevels()
        {
            var rootPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Levels");
            var folders = Directory.GetDirectories(rootPath);

            foreach (var folder in folders)
            {
                var levelXml = new FileInfo(System.IO.Path.Combine(folder, "level.xml"));
                var levelLua = new FileInfo(System.IO.Path.Combine(folder, "level.lua"));

                if (levelXml.Exists && levelLua.Exists)
                {
                    ValidateXmlDocument(levelXml.FullName, System.IO.Path.Combine(rootPath, "level.xsd"));
                    using (var stream = new FileStream(levelXml.FullName, FileMode.Open, FileAccess.Read))
                    {
                        var serializer = new XmlSerializer(typeof(LevelInfo));
                        var levelInfo = (LevelInfo) serializer.Deserialize(stream);                        
                        var sceneName = string.Format("Scenes/Maps/{0}", levelInfo.Map);

                        yield return new Level(levelInfo.Name, sceneName, levelLua.FullName);
                    }
                }
            }
        }

        private static IEnumerable<EnemyInfo> ParseEnemies()
        {
            var xmlPath = System.IO.Path.Combine(Application.streamingAssetsPath, "enemies.xml");
            var xsdPath = System.IO.Path.Combine(Application.streamingAssetsPath, "enemies.xsd");
            ValidateXmlDocument(xmlPath, xsdPath);

            using (var stream = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
            {
                var serializer = new XmlSerializer(typeof(Enemies));
                var enemies = (Enemies) serializer.Deserialize(stream);
                return enemies.Enemy;
            }
        }

        private static IEnumerable<TowerInfo> ParseTowers()
        {
            var xmlPath = System.IO.Path.Combine(Application.streamingAssetsPath, "towers.xml");
            var xsdPath = System.IO.Path.Combine(Application.streamingAssetsPath, "towers.xsd");
            ValidateXmlDocument(xmlPath, xsdPath);

            var allTowers = new List<TowerInfo>();
            using (var stream = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
            {
                var serializer = new XmlSerializer(typeof(Towers));
                var towers = (Towers) serializer.Deserialize(stream);
                allTowers.AddRange(towers.AreaOfEffectTower);
                allTowers.AddRange(towers.DirectFireTower);
            }
            return allTowers;
        }

        private static void ValidateXmlDocument(string xmlPath, string xsdPath)
        {
            var xmlDocument = new XmlDocument();
            var schemaReader = new XmlTextReader(xsdPath);
            var schema = XmlSchema.Read(schemaReader, (o, e) => { });
            xmlDocument.Load(xmlPath);
            xmlDocument.Schemas.Add(schema);
            xmlDocument.Validate((o, e) => { });

        }

        private IEnumerator LoadLevelCoroutine(Level level)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(level.SceneName, LoadSceneMode.Additive);

            while (!asyncOperation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            var scene = SceneManager.GetSceneByName(level.SceneName);
            var rootGameObject = scene.GetRootGameObjects();
            var paths = rootGameObject.SelectMany(obj => obj.GetComponentsInChildren<Path>());

            var script = new Script(CoreModules.Preset_HardSandbox);

            UserData.RegisterType<ISpawner>();
            UserData.RegisterType<IDebugger>();

            script.Globals.Set(LuaScriptConstants.DebugGlobalName, UserData.Create(new Debugger()));

            var spawners = new List<Spawner>();
            foreach (var path in paths)
            {
                var spawner = new Spawner(path.GetPath());
                spawners.Add(spawner);
                var spawnerName = string.Format("{0}{1}", path.name, LuaScriptConstants.SpawnerGlobalNameSuffix);
                script.Globals.Set(spawnerName, UserData.Create(spawner));
            }

            var scriptPath = new FileInfo(level.ScriptPath);
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
