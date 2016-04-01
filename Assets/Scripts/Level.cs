using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Level
    {
        private readonly string _name;
        private readonly GameObject _prefab;
        private readonly string _scriptPath;

        public Level([NotNull] string name, [NotNull] GameObject prefab, [NotNull] string scriptPath)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (prefab == null)
            {
                throw new ArgumentNullException("prefab");
            }
            if (scriptPath == null)
            {
                throw new ArgumentNullException("scriptPath");
            }

            _name = name;
            _prefab = prefab;
            _scriptPath = scriptPath;
        }

        public string Name
        {
            get { return _name; }
        }

        public GameObject Prefab
        {
            get { return _prefab; }
        }

        public string ScriptPath
        {
            get { return _scriptPath; }
        }
    }
}
