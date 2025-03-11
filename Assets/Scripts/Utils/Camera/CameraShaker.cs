using UnityEngine;

namespace IdleCarService.Utils
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private float _shakeAmount = 0.1f;
        [SerializeField] private float _shakeFrequency = 1f;

        private Vector3 _originalPosition;

        private void Awake()
        {
            _originalPosition = transform.position;
        }

        private void Update()
        {
            float shakeX = Mathf.PerlinNoise(Time.time * _shakeFrequency, 0f) * _shakeAmount - (_shakeAmount / 2);
            float shakeY = Mathf.PerlinNoise(0f, Time.time * _shakeFrequency) * _shakeAmount - (_shakeAmount / 2);

            transform.position = _originalPosition + new Vector3(shakeX, shakeY, 0f);
        }

        public void StartShaking()
        {
            enabled = true;
        }

        public void StopShaking()
        {
            enabled = false;
            transform.position = _originalPosition;
        }
    }
}