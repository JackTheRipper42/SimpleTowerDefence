using System;
using JetBrains.Annotations;

namespace Assets.Scripts.Lua
{
    public class SpawnAction : ISpawnerAction
    {
        private readonly string _id;
        private readonly Buff _buff;

        public SpawnAction([NotNull] string id, [NotNull] Buff buff)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (buff == null)
            {
                throw new ArgumentNullException("buff");
            }

            _id = id;
            _buff = buff;
        }

        public string Id
        {
            get { return _id; }
        }

        public Buff Buff
        {
            get { return _buff; }
        }
    }
}