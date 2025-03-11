using UnityEngine;

namespace IdleCarService.Utils
{
    [RequireComponent(typeof(CameraShaker))]
    [RequireComponent(typeof(CameraMover))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _baseFOV;
        [SerializeField] private Vector3 _cameraPosition;
        
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CameraShaker _shaker;
        [SerializeField] private CameraMover _mover;

        public void Init()
        {
            float screenRatio = (float)Screen.safeArea.height / Screen.safeArea.width;
            float referenceRatio = 1920f / 1080f;
            _mainCamera.orthographicSize = _baseFOV * (screenRatio / referenceRatio);
            
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