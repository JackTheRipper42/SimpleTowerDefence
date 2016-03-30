using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Path : MonoBehaviour
    {
        //public void SpawnEnemies()
        //{
        //    var path = GetPath();
        //    var spawner = new Spawner(GetComponentInParent<GameManager>(), path);
        //    StartCoroutine(SpawnRoutine(spawner));
        //}

        //public void CountEnemies(IDictionary<EnemyId, int> enemyCount)
        //{
        //    var spawnCounter = new SpawnCounter(enemyCount);
        //    var routine = SpawnRoutine(spawnCounter);
        //    while (routine.MoveNext())
        //    {
        //    }
        //}

        //protected abstract IEnumerator SpawnRoutine(ISpawner spawner);

        protected abstract Vector3[] GetPath();
    }
}
