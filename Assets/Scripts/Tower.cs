using Assets.Scripts.Xml;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Tower<TModel> : MonoBehaviour, ITower
        where TModel: TowerModel
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

        public virtual void Initialize(TModel model)
        {
            Id = model.Id;
            Range = model.Range;
            FireRate = model.FireRate;

            var baseSpriteRenderer = BaseTransform.GetComponent<SpriteRenderer>();
            baseSpriteRenderer.sprite = model.BaseSprite;

            var towerSpriteRenderer = TowerTransform.GetComponent<SpriteRenderer>();
            towerSpriteRenderer.sprite = model.TowerSprite;

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

        private enum State
        {
            Undefined,
            Initialized
        }

        public bool CanUpgrade()
        {
            return true;
        }

        public void Upgrade()
        {
            Debug.Log(string.Format("upgrade tower {0}", gameObject.name));
        }
    }
}
