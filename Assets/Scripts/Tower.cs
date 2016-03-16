using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Tower : MonoBehaviour
    {
        public TowerId Id;
        public float Range = 5f;       
        public float FireRate = 2f;

        private List<Enemy> _availableTargets;
        private Enemy _target;
        private float _lastShot;
        private GameManager _gameManager;

        protected virtual void Start()
        {
            _availableTargets = new List<Enemy>();
            _target = null;
            _lastShot = 0;
            var sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = Range;
            _gameManager = GetComponentInParent<GameManager>();
        }

        protected abstract void Fire(Enemy target);

        protected virtual void Update()
        {
            _availableTargets.RemoveAll(target => target == null || !target.Alive);

            if (_target != null && !_target.Alive)
            {
                _target = null;
            }

            if (_target != null)
            {
                transform.rotation = Quaternion.LookRotation(_target.transform.position - transform.position);

                var time = _gameManager.GetTime();
                if (time - _lastShot > 1f/FireRate)
                {
                    Fire(_target);
                    _lastShot = time;
                }
            }
            else
            {
                var minDistance = float.MaxValue;
                foreach (var availableTarget in _availableTargets)
                {
                    var distance = (availableTarget.transform.position - transform.position).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        _target = availableTarget;
                    }
                }
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                _availableTargets.Add(enemy);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                _availableTargets.Remove(enemy);
                if (ReferenceEquals(enemy, _target))
                {
                    _target = null;
                }
            }
        }
    }
}
