using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Path : MonoBehaviour
    {
        protected virtual void Start()
        {
            var path = GetPath();
            var spawner = new Spawner(GetComponentInParent<GameManager>(), path);
            StartCoroutine(SpawnRoutine(spawner));
        }

        protected abstract IEnumerator SpawnRoutine(ISpawner spawner);

        protected abstract Vector3[] GetPath();
    }
}
