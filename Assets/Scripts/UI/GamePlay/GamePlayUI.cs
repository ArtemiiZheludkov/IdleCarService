using IdleCarService.Core;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.GamePlay
{
    public class GamePlayUI : MonoBehaviour
    {
        [Header("VIEWS")]
        [SerializeField] private LevelView _levelView;
        [SerializeField] private MoneyView _moneyView;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private CraftView _craftView;
        
        [Header("BUTTONS")]
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _builingButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _craftButton;

        public void Init()
        {
            GameManager instance = GameManager.Instance;
            
            _levelView.Init(instance.LevelController);
            _moneyView.Init(instance.MoneyBank);
            _inventoryView.Init(instance.InventoryManager, instance.LevelController);
            _craftView.Init(instance.CraftManager, instance.InventoryManager, instance.LevelController);
        }

        public void Enable()
        {
            _levelView.Enable();
            _moneyView.Enable();
            _inventoryView.Disable();
            _craftView.Disable();
            
            gameObject.SetActive(true);
            
            _menuButton.onClick.AddListener(OnMenuClicked);
            _builingButton.onClick.AddListener(OnBuildClicked);
            _inventoryButton.onClick.AddListener(OnInventoryClicked);
            _craftButton.onClick.AddListener(OnCraftClicked);
        }

        public void Disable()
        {
            _levelView.Disable();
            _moneyView.Disable();
            _inventoryView.Disable();
            _craftView.Disable();
            
            gameObject.SetActive(false);
            
            _menuButton.onClick.RemoveListener(OnMenuClicked);
            _builingButton.onClick.RemoveListener(OnBuildClicked);
            _inventoryButton.onClick.RemoveListener(OnInventoryClicked);
            _craftButton.onClick.RemoveListener(OnBuildClicked);
        }

        private void OnMenuClicked() => GameManager.Instance.SetMenuState();
        private void OnBuildClicked() => GameManager.Instance.SetMenuState();
        private void OnInventoryClicked() => _inventoryView.Enable();
        private void OnCraftClicked() => _craftView.Enable();
    }
}