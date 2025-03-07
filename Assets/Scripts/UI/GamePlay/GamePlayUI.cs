using IdleCarService.Core;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI
{
    public class GamePlayUI : MonoBehaviour
    {
        [SerializeField] private LevelView _levelView;
        [SerializeField] private MoneyView _moneyView;
        
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _builingButton;
        [SerializeField] private Button _inventoryButton;

        public void Init()
        {
            _levelView.Init(GameManager.Instance.LevelController);
            _moneyView.Init(GameManager.Instance.MoneyBank);
        }

        public void Enable()
        {
            _levelView.Enable();
            _moneyView.Enable();
            gameObject.SetActive(true);
            
            _menuButton.onClick.AddListener(OnMenuClicked);
        }

        public void Disable()
        {
            _levelView.Disable();
            _moneyView.Disable();
            gameObject.SetActive(false);
            
            _menuButton.onClick.RemoveListener(OnMenuClicked);
        }

        private void OnMenuClicked() => GameManager.Instance.SetMenuState();
    }
}