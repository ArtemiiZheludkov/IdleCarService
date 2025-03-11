using UnityEngine;

namespace IdleCarService.Utils
{
    [RequireComponent(typeof(CameraShaker))]
    [RequireComponent(typeof(CameraMover))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 _cameraPosition;
        [SerializeField] private CameraShaker _shaker;
        [SerializeField] private CameraMover _mover;

        public void Init()
        {
            transform.position = _cameraPosition;
            
            _shaker.StopShaking();
            _mover.StopMoving();
        }

        public void SetMenuCamera()
        {
            _shaker.StartShaking(_cameraPosition);
            _mover.StopMoving();
        }

        public void SetGamePlayCamera()
        {
            _shaker.StopShaking();
            _mover.StartMoving();
        }
    }
}