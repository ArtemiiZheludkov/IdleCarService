using UnityEngine;

namespace IdleCarService.Game.Camera
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 0.5f;
        [SerializeField] private float _minZ = -10f;
        [SerializeField] private float _maxZ = 10f;

        private float _initialPosition;
        private bool _isDragging = false;
        private bool _isMouse = true;

        private void Awake()
        {
            UpdateDeviceInfo();
        }

        private void Update()
        {
            if (_isMouse)
                HandleMouseInput();
            else
                HandleTouchInput();
        }

        public void StartMoving() => enabled = true;

        public void StopMoving() =>  enabled = false;

        private void UpdateDeviceInfo()
        {
            if (Application.isEditor || SystemInfo.deviceType == DeviceType.Desktop)
                _isMouse = true;
            else if (SystemInfo.deviceType == DeviceType.Handheld)
                _isMouse = false;
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                _initialPosition = Input.mousePosition.z;
            }

            if (_isDragging && Input.GetMouseButton(0))
            {
                float deltaZ = Input.mousePosition.x - _initialPosition;
                MoveCamera(deltaZ * _moveSpeed * Time.deltaTime);
            }

            if (Input.GetMouseButtonUp(0))
                _isDragging = false;
        }
        
        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                    _initialPosition = touch.position.x;

                if (touch.phase == TouchPhase.Moved)
                {
                    float deltaZ = touch.position.x - _initialPosition;
                    MoveCamera(deltaZ * _moveSpeed * Time.deltaTime);
                }
            }
        }
        
        private void MoveCamera(float deltaZ)
        {
            Vector3 currentPosition = transform.position;
            float newZ = Mathf.Clamp(currentPosition.z + deltaZ, _minZ, _maxZ);

            transform.position = new Vector3(currentPosition.x, currentPosition.y, newZ);
        }
    }
}