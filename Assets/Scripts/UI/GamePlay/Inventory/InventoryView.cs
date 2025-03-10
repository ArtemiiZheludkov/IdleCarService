using System.Collections.Generic;
using IdleCarService.Inventory;
using IdleCarService.Progression;
using TMPro;
using UnityEngine;

namespace IdleCarService.UI.GamePlay
{
    public class InventoryView : BaseListView
    {
        [SerializeField] private ItemView _prefabItemView;
        [SerializeField] private TMP_Text _inventoryTxt;
        
        private InventoryManager _inventory;
        private Dictionary<int, ItemView> _itemViews;

        public void Init(InventoryManager inventory, LevelController levelController)
        {
            _inventory = inventory;
            base.Init(levelController);
        }

        public override void Enable()
        {
            base.Enable();
            UpdateQuantityText();
            _inventory.OnInventoryHasQuantity += OnInventoryQuantityChanged;
        }

        public override void Disable()
        {
            base.Disable();
            _inventory.OnInventoryHasQuantity -= OnInventoryQuantityChanged;
        }

        protected override void CreateViews()
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
        
        private void UpdateQuantityText() => _inventoryTxt.text = $"{_inventory.CurrentItemQuantity} / {_inventory.MaxItemQuantity}";

        private void OnInventoryQuantityChanged(bool hasQuantity) => UpdateQuantityText();
    }
}