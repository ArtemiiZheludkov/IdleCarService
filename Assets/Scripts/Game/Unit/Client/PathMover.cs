using System;
using UnityEngine;

namespace IdleCarService.Unit
{
    public class PathMover
    {
        private Mover _mover;
        private Transform[] _path;
        private int _currentIndex;

        public PathMover(Mover mover)
        {
            _mover = mover;
        }

        public void MoveAlongPath(Transform[] path, Action onPathCompleted)
        {
            _path = path;
            _currentIndex = 0;
            MoveToNextPoint(onPathCompleted);
        }

        private void MoveToNextPoint(Action onPathCompleted)
        {
            if (_currentIndex < _path.Length)
            {
                _mover.MoveTo(_path[_currentIndex], () =>
                {
                    _currentIndex++;
                    MoveToNextPoint(onPathCompleted);
                });
            }
            else
            {
                onPathCompleted?.Invoke();
            }
        }
    }
}