using IdleCarService.Core;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayClicked);
            exitButton.onClick.AddListener(OnExitClicked);
        }

        private void OnPlayClicked()
        {
            SceneLoader.LoadSceneAsync(SceneLoader.Scene.GamePlay);
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