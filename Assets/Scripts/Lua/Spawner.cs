using MoonSharp.Interpreter;
using System.Collections.Generic;
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

        public bool Finished { get; set; }

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
            var id = table.Get(LuaScriptConstants.IdKey).String;
            var healthBoostDynValue = table.Get(LuaScriptConstants.HealthBoostKey);
            var healthBoost = healthBoostDynValue.IsNil() ? 1f : (float) healthBoostDynValue.Number;
            _actions.Add(new SpawnAction(id, new Buff(healthBoost)));
        }

        public void Clear()
        {
            _actions.Clear();
        }
    }
}
