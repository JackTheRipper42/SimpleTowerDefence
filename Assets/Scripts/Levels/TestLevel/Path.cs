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
                    spawner.SpawnEnemy();
                    yield return new WaitForSeconds(0.25f);
                }
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}
