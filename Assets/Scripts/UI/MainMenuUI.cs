using IdleCarService.Core;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;

        private void OnEnable()
        {
            playButton.onClick.AddListener(OnPlayClicked);
            exitButton.onClick.AddListener(OnExitClicked);
        }
        
        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnPlayClicked);
            exitButton.onClick.RemoveListener(OnExitClicked);
        }
        
        public void Enable() => gameObject.SetActive(true);

        public void Disable() => gameObject.SetActive(false);

        private void OnPlayClicked()
        {
            GameManager.Instance.SetGamePlayState();
        }

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