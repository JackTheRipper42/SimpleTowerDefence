using Assets.Scripts.Lua;
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
        public RuntimeAnimatorController[] AnimatorControllers;
        public Sprite[] Sprites;
        public GameObject EnemyPrefab;
        public GameObject DirectFireTowerPrefab;
        public GameObject AreaOfEffectTowerPrefab;
        [Range(0f, 2f)] public float MaxSpawnOffset = 1f;
        public int Cash;

        private IDictionary<string, Sprite> _sprites; 
        private IDictionary<string, RuntimeAnimatorController> _animatorControllers;
        private IDictionary<string, EnemyInfo> _enemyInfos;
        private IDictionary<string, ITowerInfo> _towerInfos;
        private IDictionary<string, int[]> _towerPrices; 
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

        public void SpawnTower(string id, Vector3 position)
        {
            var info = _towerInfos[id];
            var model = ParserTowerInfo(info, _sprites);
            var modelType = model.GetType();

            GameObject obj;
            if (modelType == typeof(DirectFireTowerModel))
            {
                obj = Instantiate(DirectFireTowerPrefab);
                var tower = obj.GetComponent<DirectFireTower>();
                tower.Initialize((DirectFireTowerModel) model);
            }
            else if (modelType == typeof(AreaOfEffectTowerModel))
            {
                obj = Instantiate(AreaOfEffectTowerPrefab);
                var tower = obj.GetComponent<AreaOfEffectTower>();
                tower.Initialize((AreaOfEffectTowerModel) model);
            }
            else
            {
                throw new NotSupportedException(string.Format(
                    "The tower type '{0}' is not supported.",
                    modelType));
            }
            obj.transform.parent = TowerContainer.transform;
            obj.transform.position = position;

            Cash -= _towerPrices[id][0];
        }

        public bool CanSpawnTower(string id, Vector3 position)
        {
            return !_towerPositions.Contains(position) && Cash >= _towerPrices[id][0];
        }

        public void UpgradeTower(ITower tower)
        {
            tower.Upgrade();
            Cash -= _towerPrices[tower.Id][tower.Level];
        }

        public bool CanUpgrade(ITower tower)
        {
            return tower.CanUpgrade() && Cash >= _towerPrices[tower.Id][tower.Level + 1];
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

        public IEnumerable<string> GetTowerIds()
        {
            return _towerInfos.Keys;
        }

        protected virtual void Start()
        {
            _enemies = new HashSet<Enemy>();
            _towerPositions = new HashSet<Vector3>();

            _sprites = Sprites.ToDictionary(
                sprite => sprite.name,
                sprite => sprite);
            _animatorControllers = AnimatorControllers.ToDictionary(
                controller => controller.name,
                controller => controller);
            _enemyInfos = ParseEnemies().ToDictionary(
                enemy => enemy.Id,
                enemy => enemy);
            _towerInfos = new Dictionary<string, ITowerInfo>();
            _towerPrices = new Dictionary<string, int[]>();
            foreach (var towerInfo in ParseTowers())
            {
                _towerInfos.Add(towerInfo.Id, towerInfo);
                _towerPrices.Add(
                    towerInfo.Id,
                    towerInfo.Levels.Select(level => level.Price).ToArray());
            }
          
            var levels = ParseLevels();
            LoadLevel(levels.First());
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

        private void SpawnEnemy(string id, IList<Vector3> path)
        {
            var obj = Instantiate(EnemyPrefab);
            var enemy = obj.GetComponent<Enemy>();
            obj.transform.parent = EnemyContainer.transform;
            var offset = Random.insideUnitSphere*MaxSpawnOffset;
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

                        yield return new Level(levelInfo.Name, sceneName, levelLua.FullName, levelInfo.Waves);
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

        private static IEnumerable<ITowerInfo> ParseTowers()
        {
            var xmlPath = System.IO.Path.Combine(Application.streamingAssetsPath, "towers.xml");
            var xsdPath = System.IO.Path.Combine(Application.streamingAssetsPath, "towers.xsd");
            ValidateXmlDocument(xmlPath, xsdPath);

            var allTowers = new List<ITowerInfo>();
            using (var stream = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
            {
                var serializer = new XmlSerializer(typeof(Towers));
                var towers = (Towers) serializer.Deserialize(stream);
                allTowers.AddRange(towers.DirectFireTower);
                allTowers.AddRange(towers.AreaOfEffectTower);
            }
            return allTowers;
        }

        private static ITowerModel ParserTowerInfo(ITowerInfo info, IDictionary<string, Sprite> sprites)
        {
            var infoType = info.GetType();
            if (infoType == typeof(DirectFireTowerInfo))
            {
                var directFireTowerInfo = (DirectFireTowerInfo) info;
                return new DirectFireTowerModel(
                    directFireTowerInfo.Id,
                    sprites[directFireTowerInfo.BaseSprite],
                    directFireTowerInfo.Levels.Select(level => ParseTowerLevel(level, sprites)).ToArray());
            }
            if (infoType == typeof(AreaOfEffectTowerInfo))
            {
                var areaOfEffectTowerInfo = (AreaOfEffectTowerInfo) info;
                return new AreaOfEffectTowerModel(
                    areaOfEffectTowerInfo.Id,
                    sprites[areaOfEffectTowerInfo.BaseSprite],
                    areaOfEffectTowerInfo.Levels.Select(level => ParseTowerLevel(level, sprites)).ToArray());
            }
            throw new NotSupportedException(string.Format(
                "The tower type '{0}' is not supported.",
                infoType));
        }

        private static DirectFireTowerLevel ParseTowerLevel(
            DirectFireTowerLevelInfo info,
            IDictionary<string, Sprite> sprites)
        {
            return new DirectFireTowerLevel(
                info.Price,
                info.Range,
                info.FireRate,
                info.Damage,
                sprites[info.TowerSprite]);
        }

        private static AreaOfEffectTowerLevel ParseTowerLevel(
            AreaOfEffectTowerLevelInfo info,
            IDictionary<string, Sprite> sprites)
        {
            return new AreaOfEffectTowerLevel(
                info.Price,
                info.Range,
                info.FireRate,
                info.AreaDamage,
                info.DamageRange,
                sprites[info.TowerSprite]);
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

            script.Options.DebugPrint = Debug.Log;
            UserData.RegisterType<ISpawner>();

            var spawners = new List<Spawner>();
            foreach (var path in paths)
            {
                var spawner = new Spawner(path.GetPath());
                spawners.Add(spawner);
                var spawnerName = CreateSpawnerName(path.name);
                script.Globals.Set(spawnerName, UserData.Create(spawner));
            }

            var scriptPath = new FileInfo(level.ScriptPath);
            var scriptCode = File.ReadAllText(scriptPath.FullName);
            script.DoString(scriptCode);

            var getCashFunction = script.Globals.Get(LuaScriptConstants.GetCashFunctionName);
            if (getCashFunction.IsNil())
            {
                throw new InvalidOperationException(string.Format(
                    "The '{0}' function does not exist in script '{1}'.",
                    LuaScriptConstants.GetCashFunctionName,
                    scriptPath.Name));
            }
            var setupSpawnFunction = script.Globals.Get(LuaScriptConstants.SetupWaveFunctionName);
            if (setupSpawnFunction.IsNil())
            {
                throw new InvalidOperationException(string.Format(
                    "The '{0}' function does not exist in script '{1}'.",
                    LuaScriptConstants.SetupWaveFunctionName,
                    scriptPath.Name));
            }

            for (var wave = 0; wave < level.Waves; wave++)
            {
                foreach (var spawner in spawners)
                {
                    spawner.Clear();
                }

                var cashResult = script.Call(script.Globals[LuaScriptConstants.GetCashFunctionName], wave);
                Cash += (int) cashResult.Number;
                script.Call(script.Globals[LuaScriptConstants.SetupWaveFunctionName], wave);

                yield return new WaitForSeconds(5);
                foreach (var spawner in spawners)
                {
                    StartCoroutine(SpawnCoroutine(spawner));
                }

                while (spawners.Any(spawner => !spawner.Finished) || _enemies.Count > 0)
                {
                    yield return new WaitForEndOfFrame();
                }

                Debug.Log("wave finished");
            }

            Debug.Log("level finished");

            yield return new WaitForSeconds(4);

            UnloadLevel();
        }

        private static string CreateSpawnerName(string pathName)
        {
            if (pathName.IndexOfAny(LuaScriptConstants.DigitCharacters) == 0)
            {
                throw new InvalidOperationException(string.Format(
                    "The name '{0}' starts with a digit character.",
                    pathName));
            }
            if (pathName.IndexOfAny(LuaScriptConstants.InvalidNameCharacters) != -1)
            {
                throw new InvalidOperationException(string.Format(
                    "The name '{0}' contains invalid character.", 
                    pathName));
            }

            var luaPathName = char.ToLowerInvariant(pathName[0]) + pathName.Substring(1);
            return string.Format("{0}{1}", luaPathName, LuaScriptConstants.SpawnerGlobalNameSuffix);
        }

        private IEnumerator SpawnCoroutine(Spawner spawner)
        {
            spawner.Finished = false;
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
            spawner.Finished = true;
        }
    }
}
