using JetBrains.Annotations;
using System;

namespace Assets.Scripts
{
    public class Level
    {
        private readonly string _name;
        private readonly string _sceneName;
        private readonly string _scriptPath;
        private readonly int _waves;

        public Level(
            [NotNull] string name, 
            [NotNull] string sceneName, 
            [NotNull] string scriptPath,
            int waves)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (sceneName == null)
            {
                throw new ArgumentNullException("sceneName");
            }
            if (scriptPath == null)
            {
                throw new ArgumentNullException("scriptPath");
            }

            _name = name;
            _sceneName = sceneName;
            _scriptPath = scriptPath;
            _waves = waves;
        }

        public string Name
        {
            get { return _name; }
        }

        public string SceneName
        {
            get { return _sceneName; }
        }

        public string ScriptPath
        {
            get { return _scriptPath; }
        }

        public int Waves
        {
            get { return _waves; }
        }
    }
}
