using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Scripts.Lua
{
    public class Spawner : ISpawner
    {
        private readonly IList<Vector3> _path;
        private readonly List<ISpawnerAction> _actions;

        public Spawner(IList<Vector3> path)
        {
            _path = path;
            _actions = new List<ISpawnerAction>();
        }

        public IEnumerable<ISpawnerAction> Actions
        {
            get { return _actions; }
        }

        public IList<Vector3> Path
        {
            get { return _path; }
        }

        public void Wait(double time)
        {
            _actions.Add(new WaitAction(time));
        }

        public void Debug(string message)
        {
            _actions.Add(new DebugAction(message));
        }

        public void Spawn(Table table)
        {
            var id = table.Get("id").String;
            _actions.Add(new SpawnAction(id));
        }
    }
}
