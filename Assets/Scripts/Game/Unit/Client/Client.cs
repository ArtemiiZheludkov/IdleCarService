using System;
using UnityEngine;
using IdleCarService.Build;

namespace IdleCarService.Unit
{
    [RequireComponent(typeof(BoxCollider))]
    public class Client : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        
        private float _speed;
        private Mover _mover;
        private PathMover _pathMover;
        
        private Transform _mainTarget;
        private Action<Client> _onMainTargetFinished;
        
        private bool _isMoving;
        private ServiceStation _currentStation;

        public void Init(float speed, float rotateSpeed)
        {
            _speed = speed;
            
            _isMoving = false;
            _mover = new Mover(transform, _speed, rotateSpeed);
            _pathMover = new PathMover(_mover);
            
            _collider.enabled = false;
        }

        private void FixedUpdate()
        {
            if (_isMoving)
                _mover.FixedUpdate();
        }
        
        public void StopMoving()
        {
            _isMoving = false;
        }

        public void InitForRoad(Transform mainTarget, Action<Client> onMainTargetFinished)
        {
            _mainTarget = mainTarget;
            _onMainTargetFinished = onMainTargetFinished;
            
            _collider.enabled = true;
            _isMoving = true;
            _mover.MoveTo(_mainTarget, OnMainTargetFinished);
        }
        
        public bool EnterService(ServiceStation station)
        {
            if (station == null || station.HasClient)
                return false;

            _collider.enabled = false;
            _currentStation = station;
            _pathMover.MoveAlongPath(_currentStation.EnterPath, OnServiceEntered);
            
            return true;
        }

        private void OnMainTargetFinished()
        {
            _isMoving = false;
            _onMainTargetFinished?.Invoke(this);
        }

        private void OnServiceEntered()
        {
            if (_currentStation == null)
                return;
            
            _isMoving = false;
            _currentStation.CreateServiceForClient(OnServiceCompleted);
        }

        private void OnServiceCompleted()
        {
            if (_currentStation == null)
                return;
            
            _isMoving = true;
            _pathMover.MoveAlongPath(_currentStation.ExitPath, OnServiceExit);
        }

        private void OnServiceExit()
        {
            if (_mainTarget == null)
                return;
            
            _mover.MoveTo(_mainTarget, OnMainTargetFinished);
        }
    }
}
