using IdleCarService.UI.GamePlay;
using IdleCarService.UI.MainMenu;
using UnityEngine;

namespace IdleCarService.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MainMenuUI _menuUI;
        [SerializeField] private GamePlayUI _gamePlayUI;

        public void Init()
        {
            _gamePlayUI.Init();
        }

        public void SetMenuUI()
        {
            _gamePlayUI.Disable();
            _menuUI.Enable();
        }
        
        public void SetGamePlayUI()
        {
            _menuUI.Disable();
            _gamePlayUI.Enable();
        }
    }
}