using Assets.Scripts.Xml;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Tower<TInfo> : MonoBehaviour
        where TInfo: TowerInfo
    {
        public Transform BaseTransform;
        public Transform TowerTransform;

        private Enemy _target;
        private float _lastShot;
        private GameManager _gameManager;
        private State _state;

        public TowerId Id { get; private set; }
        public float Range { get; private set; }
        public float FireRate { get; private set; }

        protected Tower()
        {
            _state = State.Undefined;
        }

        public virtual void Initialize(TInfo towerInfo, Sprite baseSprite, Sprite towerSprite)
        {
            Id = towerInfo.Id;
            Range = towerInfo.Range;
            FireRate = towerInfo.FireRate;

            var baseSpriteRenderer = BaseTransform.GetComponent<SpriteRenderer>();
            baseSpriteRenderer.sprite = baseSprite;

            var towerSpriteRenderer = TowerTransform.GetComponent<SpriteRenderer>();
            towerSpriteRenderer.sprite = towerSprite;

            var rangeRenderer = GetComponentInChildren<RangeRenderer>();
            rangeRenderer.Initialize(Range);

            _state = State.Initialized;
        }

        protected virtual void Start()
        {
            _target = null;
            _lastShot = 0;
            _gameManager = GetComponentInParent<GameManager>();
        }

        protected abstract void Fire(Enemy target);

        protected virtual void Update()
        {
            if (_state == State.Undefined)
            {
                return;
            }

            var availableTargets = _gameManager.GetEnemies().Where(IsInRange).ToList();

            if (_target != null && (!_target.Alive || !IsInRange(_target)))
            {
                _target = null;
            }

            if (_target != null)
            {
                var direction = Quaternion.LookRotation(_target.transform.position - transform.position).eulerAngles;
                TowerTransform.rotation = Quaternion.Euler(90f, direction.y, 0f);

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
                foreach (var availableTarget in availableTargets)
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

        private bool IsInRange(Enemy enemy)
        {
            var distance = (enemy.Position - transform.position).magnitude;
            return distance <= Range;
        }

        //protected virtual void OnTriggerEnter(Collider other)
        //{
        //    var enemy = other.GetComponentInParent<Enemy>();
        //    if (enemy != null)
        //    {
        //        _availableTargets.Add(enemy);
        //    }
        //}

        //protected virtual void OnTriggerExit(Collider other)
        //{
        //    var enemy = other.GetComponentInParent<Enemy>();
        //    if (enemy != null)
        //    {
        //        _availableTargets.Remove(enemy);
        //        if (ReferenceEquals(enemy, _target))
        //        {
        //            _target = null;
        //        }
        //    }
        //}

        private enum State
        {
            Undefined,
            Initialized
        }
    }
}
