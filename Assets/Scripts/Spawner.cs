using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Spawner : ISpawner
    {
        private readonly GameManager _gameManager;
        private readonly Vector3[] _path;

        public Spawner([NotNull] GameManager gameManager, [NotNull] Vector3[] path)
        {
            if (gameManager == null)
            {
                throw new ArgumentNullException("gameManager");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            _gameManager = gameManager;
            _path = path;
        }

        public void SpawnEnemy(EnemyId id)
        {
            _gameManager.SpawnEnemy(id, _path);
        }
    }
}
