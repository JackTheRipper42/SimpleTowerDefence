using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Tower<TModel, TLevel> : MonoBehaviour, ITower
        where TModel: TowerModel<TLevel>
        where TLevel : TowerLevel
    {
        public Transform BaseTransform;
        public Transform TowerTransform;

        private Enemy _target;
        private float _lastShot;
        private GameManager _gameManager;
        private State _state;

        protected Tower()
        {
            _state = State.Undefined;
        }

        public TowerId Id
        {
            get { return Model.Id; }
        }

        public float Range
        {
            get { return Model.Levels[Level].Range; }
        }

        public float FireRate
        {
            get { return Model.Levels[Level].FireRate; }
        }

        public int Level
        {
            get { return Model.LevelProperty.GetValue(); }
            private set { Model.LevelProperty.SetValue(value); }
        }

        public TModel Model { get; private set; }

        public bool CanUpgrade()
        {
            return Level < Model.Levels.Length - 1;
        }

        public void Upgrade()
        {
            Level++;
        }

        public virtual void Initialize(TModel model)
        {
            Model = model;

            var baseSpriteRenderer = BaseTransform.GetComponent<SpriteRenderer>();
            baseSpriteRenderer.sprite = model.BaseSprite;

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
    }
}
