using System.Collections.Generic;
using IdleCarService.Inventory;
using IdleCarService.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.GamePlay
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private ItemView _prefabItemView;
        [SerializeField] private Transform _contentParent;
        [SerializeField] private Button _closeButton;
        
        private InventoryManager _inventory;
        
        private Dictionary<int, ItemView> _itemViews;
        
        public void Init(InventoryManager inventoryManager, LevelController levelController)
        {
            _inventory = inventoryManager;
            
            CreateViews();
            
            levelController.LevelChanged += OnLevelChanged;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            _closeButton.onClick.AddListener(Disable);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            _closeButton.onClick.RemoveListener(Disable);
        }

        private void CreateViews()
        {
            if (_itemViews == null)
                _itemViews = new Dictionary<int, ItemView>();

            List<ItemConfig> unlockedConfigs = _inventory.GetUnlockedItems();

            foreach (ItemConfig config in unlockedConfigs)
            {
                if (_itemViews.ContainsKey(config.Id) == false)
                    CreateItemView(config);
            }
        }

        private void CreateItemView(ItemConfig config)
        {
            ItemView itemView = Instantiate(_prefabItemView, _contentParent);
            itemView.Init(config, _inventory);
            _itemViews.Add(config.Id, itemView);
        }

        private void OnLevelChanged(int level) => CreateViews();
    }
}