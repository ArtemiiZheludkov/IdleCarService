using IdleCarService.UI;
using IdleCarService.Utils;
using UnityEngine;

namespace IdleCarService.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private UIManager _uiManager;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            SetMenuState();
        }

        public void SetMenuState()
        {
            _uiManager.SetMenuUI();
            _cameraShaker.StartShaking();
        }

        public void SetGamePlayState()
        {
            _uiManager.SetGamePlayUI();
            _cameraShaker.StopShaking();
        }
    }
}