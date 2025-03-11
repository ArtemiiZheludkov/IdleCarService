using System;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;

        private Action _setGamePlayState;
        private Action _closeGame;
        
        public void Init(Action setGamePlayState, Action closeGame)
        {
            _setGamePlayState = setGamePlayState;
            _closeGame = closeGame;
        }

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

        private void OnPlayClicked() => _setGamePlayState?.Invoke();

        private void OnExitClicked() => _closeGame?.Invoke();
    }

}