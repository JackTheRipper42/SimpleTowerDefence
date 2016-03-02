using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public float Speed = 10;
        public float MaxHealth = 10;

        private float _lerpLength;
        private float _lerpPosition;
        private int _currentIndex;
        private float _health;
        private Vector3[] _path;
        private State _state;
        private GameManager _gameManager;

        public bool Alive
        {
            get { return _health > 0; }
        }

        public void SetPath(IEnumerable<Vector3> path)
        {
            _path = path.ToArray();
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
            _health -= effectiveDamage;
            if (_health <= 0f)
            {
                _state = State.Killed;
                _gameManager.EnemyKilled(this);
            }
        }

        protected virtual void Start()
        {
            _health = MaxHealth;
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
                    _lerpPosition += (effectiveSpeed * deltaTime) / _lerpLength;
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
                            _gameManager.EnemyExists(this);
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
            _lerpPosition = 0;
            _lerpLength = (_path[index + 1] - _path[index]).magnitude;
            transform.position = _path[index];
        }

        private void LerpPosition(float t)
        {
            transform.position = Vector3.Lerp(_path[_currentIndex], _path[_currentIndex + 1], t);
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
