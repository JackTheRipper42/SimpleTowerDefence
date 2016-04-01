using JetBrains.Annotations;
using System;

namespace Assets.Scripts
{
    public class Level
    {
        private readonly string _name;
        private readonly string _map;
        private readonly string _scriptPath;

        public Level([NotNull] string name, string map, [NotNull] string scriptPath)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (scriptPath == null)
            {
                throw new ArgumentNullException("scriptPath");
            }

            _name = name;
            _map = map;
            _scriptPath = scriptPath;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Map
        {
            get { return _map; }
        }

        public string ScriptPath
        {
            get { return _scriptPath; }
        }
    }
}
