using System;
using UnityEngine;

namespace IdleCarService.Unit
{
    public class Mover
    {
        private Transform _transform;
        private float _speed, _rotateSpeed;

        private Transform _target;
        private Action _onFinish;

        public Mover(Transform transform, float speed, float rotateSpeed)
        {
            _transform = transform;
            _speed = speed;
            _rotateSpeed = rotateSpeed;
        }

        public void FixedUpdate()
        {
            if (_target == null)
                return;

            Vector3 direction = (_target.position - _transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);
            }

            _transform.position = Vector3.MoveTowards(_transform.position, _target.position, 
                _speed * Time.fixedDeltaTime);

            if (Vector3.Distance(_transform.position, _target.position) < 0.1f)
                _onFinish?.Invoke();
        }

        public void MoveTo(Transform target, Action onTargetFinished)
        {
            _target = target;
            _onFinish = onTargetFinished;
        }
    }
}