using UnityEngine;

namespace IdleCarService.Game.Camera
{
    [RequireComponent(typeof(CameraShaker))]
    [RequireComponent(typeof(CameraMover))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CameraShaker _shaker;
        [SerializeField] private CameraMover _mover;

        public void Init()
        {
            _shaker.StopShaking();
            _mover.StopMoving();
        }

        public void SetMenuCamera()
        {
            _shaker.StartShaking();
            _mover.StopMoving();
        }

        public void SetGamePlayCamera()
        {
            _shaker.StopShaking();
            _mover.StartMoving();
        }
    }
}