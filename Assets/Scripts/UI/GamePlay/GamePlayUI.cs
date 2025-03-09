using System;
using IdleCarService.Build;
using IdleCarService.Craft;
using IdleCarService.Inventory;
using IdleCarService.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.GamePlay
{
    public class GamePlayUI : MonoBehaviour
    {
        [Header("VIEWS")]
        [SerializeField] private LevelView _levelView;
        [SerializeField] private MoneyView _moneyView;
        [SerializeField] private BuildView _buildView;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private CraftView _craftView;
        
        [Header("BUTTONS")]
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _builingButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _craftButton;

        private Action _setMenuState;

        public void Init(LevelController level, MoneyBank bank, BuildingManager builder, 
            InventoryManager inventory, CraftManager crafter, Action setMenuState)
        {
            _setMenuState = setMenuState;
            
            _levelView.Init(level);
            _moneyView.Init(bank);
            _buildView.Init(builder, bank, level);
            _inventoryView.Init(inventory, level);
            _craftView.Init(crafter, inventory, level);
        }

        public void Enable()
        {
            _levelView.Enable();
            _moneyView.Enable();
            _buildView.Disable();
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
            _buildView.Disable();
            _inventoryView.Disable();
            _craftView.Disable();
            
            gameObject.SetActive(false);
            
            _menuButton.onClick.RemoveListener(OnMenuClicked);
            _builingButton.onClick.RemoveListener(OnBuildClicked);
            _inventoryButton.onClick.RemoveListener(OnInventoryClicked);
            _craftButton.onClick.RemoveListener(OnBuildClicked);
        }

        private void OnMenuClicked() => _setMenuState?.Invoke();
        private void OnBuildClicked() => _buildView.Enable();
        private void OnInventoryClicked() => _inventoryView.Enable();
        private void OnCraftClicked() => _craftView.Enable();
    }
}