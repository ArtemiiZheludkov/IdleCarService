using IdleCarService.Core;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;
        
        public void Enable()
        {
            gameObject.SetActive(true);
            
            _playButton.onClick.AddListener(OnPlayClicked);
            _exitButton.onClick.AddListener(OnExitClicked);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            
            _playButton.onClick.RemoveListener(OnPlayClicked);
            _exitButton.onClick.RemoveListener(OnExitClicked);
        }

        private void OnPlayClicked() => GameManager.Instance.SetGamePlayState();

        private void OnExitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

}