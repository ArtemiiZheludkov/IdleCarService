using UnityEngine;

namespace IdleCarService.Utils
{
    public class CameraShaker : MonoBehaviour
    {
        public float shakeAmount = 0.1f;
        public float shakeFrequency = 1f;

        private Vector3 originalPosition;

        private void Awake()
        {
            originalPosition = transform.position;
            StartShaking();
        }

        private void Update()
        {
            float shakeX = Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) * shakeAmount - (shakeAmount / 2);
            float shakeY = Mathf.PerlinNoise(0f, Time.time * shakeFrequency) * shakeAmount - (shakeAmount / 2);

            transform.position = originalPosition + new Vector3(shakeX, shakeY, 0f);
        }

        public void StartShaking()
        {
            enabled = true;
        }

        public void StopShaking()
        {
            enabled = false;
            transform.position = originalPosition;
        }
    }
}