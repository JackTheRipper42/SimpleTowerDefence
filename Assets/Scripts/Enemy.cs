using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public float Speed = 10;

        private float _lerpLength;
        private float _lerpPosition;
        private int _currentIndex;
        private Vector3[] _path;
        private PathFollowerState _state;
        private GameManager _gameManager;

        public void SetPath(IEnumerable<Vector3> path)
        {
            _path = path.ToArray();
            InitState();
        }

        public void ResetPath()
        {
            _path = null;
            _state = PathFollowerState.Undefinded;
        }

        protected virtual void Start()
        {
            _gameManager = GetComponentInParent<GameManager>();
            InitState();
        }

        protected virtual void Update()
        {
            switch (_state)
            {
                case PathFollowerState.Initialize:
                    InitLerp(0);
                    _state = PathFollowerState.Running;
                    break;
                case PathFollowerState.Running:
                    var effectiveSpeed = CalculateEffectiveSpeed();
                    var deltaTime = GetDeltaTime();
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
                            _state = PathFollowerState.Finished;
                        }
                    }
                    break;
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "Exit")
            {
                _gameManager.EnemyExists(this);
            }
        }

        protected virtual float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        protected virtual float CalculateEffectiveSpeed()
        {
            return Speed;
        }

        private void InitState()
        {
            if (_path != null && _path.Length >= 2)
            {
                _state = PathFollowerState.Initialize;
            }
            else
            {
                _state = PathFollowerState.Undefinded;
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

        private enum PathFollowerState
        {
            Undefinded,
            Initialize,
            Running,
            Finished
        }
    }
}
