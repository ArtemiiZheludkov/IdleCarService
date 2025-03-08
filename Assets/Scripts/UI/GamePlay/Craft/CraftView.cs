using System.Collections.Generic;
using IdleCarService.Craft;
using IdleCarService.Inventory;
using IdleCarService.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.GamePlay
{
    public class CraftView : MonoBehaviour
    {
        [SerializeField] private CraftItemView _prefabItemView;
        [SerializeField] private Transform _contentParent;
        [SerializeField] private Button _closeButton;
        
        private CraftManager _craft;
        private InventoryManager _inventory;
        
        private Dictionary<int, CraftItemView> _itemViews;
        
        public void Init(CraftManager craftManager, InventoryManager inventoryManager, LevelController levelController)
        {
            _craft = craftManager;
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
                _itemViews = new Dictionary<int, CraftItemView>();

            List<ItemConfig> unlockedConfigs = _craft.GetUnlockedCraftableItems();

            foreach (ItemConfig config in unlockedConfigs)
            {
                if (_itemViews.ContainsKey(config.Id) == false)
                    CreateItemView(config);
            }
        }

        private void CreateItemView(ItemConfig config)
        {
            CraftItemView itemView = Instantiate(_prefabItemView, _contentParent);
            itemView.Init(config, _craft, _inventory);
            _itemViews.Add(config.Id, itemView);
        }

        private void OnLevelChanged(int level) => CreateViews();
    }
}