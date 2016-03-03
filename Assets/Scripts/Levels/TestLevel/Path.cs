using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Levels.TestLevel
{
    public class Path : AutoPath
    {
        protected override IEnumerator SpawnRoutine(ISpawner spawner)
        {
            yield return new WaitForSeconds(4f);

            for (var i = 0; i < 20; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    spawner.SpawnEnemy(EnemyId.SmallWalker);
                    yield return new WaitForSeconds(0.25f);
                }
                yield return new WaitForSeconds(1.2f);
                spawner.SpawnEnemy(EnemyId.BigWalker);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
