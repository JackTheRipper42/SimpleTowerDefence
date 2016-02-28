using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Turret : MonoBehaviour
    {
        public float Range = 5f;
        public float Damage = 0.3f;
        public float FireRate = 2f;

        private List<Enemy> _availableTargets;
        private Enemy _target;
        private float _lastShot;


        protected virtual void Start()
        {
            _availableTargets = new List<Enemy>();
            _target = null;
            _lastShot = 0;
            var sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = Range;
        }

        protected virtual void Update()
        {
            if (_target != null && !_target.Alive)
            {
                _target = null;
            }

            if (_target != null)
            {
                transform.rotation = Quaternion.LookRotation(_target.transform.position - transform.position);

                if (Time.time - _lastShot > 1f/FireRate)
                {
                    Debug.Log("Peng");
                    _target.SetHit(Damage);
                    _lastShot = Time.time;
                }
            }
            else
            {
                var minDistance = float.MaxValue;
                foreach (var availableTarget in _availableTargets.Where(target => target.Alive))
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
            var enemy = other.GetComponent<Enemy>();
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
