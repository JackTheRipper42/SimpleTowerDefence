using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class SpawnCounter : ISpawner
    {
        private readonly IDictionary<EnemyId, int> _enemyCount;

        public SpawnCounter([NotNull] IDictionary<EnemyId, int> enemyCount)
        {
            if (enemyCount == null)
            {
                throw new ArgumentNullException("enemyCount");
            }

            _enemyCount = enemyCount;
        }

        public void SpawnEnemy(EnemyId id)
        {
            int count;
            if (_enemyCount.TryGetValue(id, out count))
            {
                _enemyCount[id] = count + 1;
            }
            else
            {
                _enemyCount.Add(id, 1);
            }
        }
    }
}
