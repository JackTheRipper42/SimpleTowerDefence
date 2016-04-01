using Assets.Scripts.Binding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public readonly NotifyingObject<float> HealthProperty;
        public readonly NotifyingObject<Vector3> PositionProperty; 

        public EnemyId Id;
        public float Speed = 10;
        public float MaxHealth = 10;
        public Transform RendererTransform;

        private float _lerpLength;
        private float _lerpPosition;
        private int _currentIndex;
        private Vector3[] _path;
        private Vector3 _offset;
        private State _state;
        private GameManager _gameManager;

        public Enemy()
        {
            HealthProperty = new NotifyingObject<float>();
            PositionProperty = new NotifyingObject<Vector3>();
        }

        public bool Alive
        {
            get { return Health > 0; }
        }

        public float Health
        {
            get { return HealthProperty.GetValue(); }
            private set { HealthProperty.SetValue(value); }
        }

        public Vector3 Position
        {
            get { return PositionProperty.GetValue(); }
            set
            {
                transform.position = value;
                PositionProperty.SetValue(value);
            }
        }

        public void SetPath(IEnumerable<Vector3> path, Vector3 offset)
        {
            _path = path.ToArray();
            _offset = offset;
            InitState();
        }

        public void ResetPath()
        {
            _path = null;
            _state = State.Undefinded;
        }

        public void SetHit(float damage)
        {
            var effectiveDamage = Mathf.Max(0f, CalculateEffectiveDamage(damage));
            Health -= effectiveDamage;
            if (Health <= 0f)
            {
                _state = State.Killed;
                _gameManager.EnemyKilled(this);
            }
        }

        protected virtual void Start()
        {
            Health = MaxHealth;
            _gameManager = GetComponentInParent<GameManager>();
            InitState();
        }

        protected virtual void Update()
        {
            switch (_state)
            {
                case State.Initialize:
                    InitLerp(0);
                    _state = State.Running;
                    break;
                case State.Running:
                    var effectiveSpeed = CalculateEffectiveSpeed();
                    var deltaTime = _gameManager.GetDeltaTime();
                    _lerpPosition += (effectiveSpeed*deltaTime)/_lerpLength;
                    LerpPosition(_lerpPosition);
                    if (_lerpPosition >= 1.0f)
                    {
                        if (_currentIndex < _path.Length - 2)
                        {
                            InitLerp(_currentIndex + 1);
                        }
                        else
                        {
                            _state = State.Finished;
                            _gameManager.EnemyExits(this);
                        }
                    }
                    break;
            }
        }

        protected virtual float CalculateEffectiveSpeed()
        {
            return Speed;
        }

        protected virtual float CalculateEffectiveDamage(float damage)
        {
            return damage;
        }

        private void InitState()
        {
            if (_path != null && _path.Length >= 2)
            {
                _state = State.Initialize;
            }
            else
            {
                _state = State.Undefinded;
            }
        }

        private void InitLerp(int index)
        {
            _currentIndex = index;
            var vector = _path[index + 1] - _path[index];
            _lerpPosition = 0;
            _lerpLength = vector.magnitude;
            transform.position = _path[index] + _offset;
            var direction = Quaternion.LookRotation(vector).eulerAngles;
            RendererTransform.rotation = Quaternion.Euler(90f, direction.y - 90f, 0f);
        }

        private void LerpPosition(float t)
        {
            Position = Vector3.Lerp(_path[_currentIndex], _path[_currentIndex + 1], t) + _offset;
        }

        private enum State
        {
            Undefinded,
            Initialize,
            Running,
            Finished,
            Killed
        }
    }
}
