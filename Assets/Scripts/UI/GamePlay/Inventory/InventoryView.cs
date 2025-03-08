using System.Collections.Generic;
using IdleCarService.Inventory;
using UnityEngine;

namespace IdleCarService.UI.GamePlay
{
    public class InventoryView : BaseItemView
    {
        [SerializeField] private ItemView _prefabItemView;
        
        private Dictionary<int, ItemView> _itemViews;
        
        protected override void CreateViews()
        {
            if (_itemViews == null)
                _itemViews = new Dictionary<int, ItemView>();

            List<ItemConfig> unlockedConfigs = GetUnlockedItems();

            foreach (ItemConfig config in unlockedConfigs)
            {
                if (_itemViews.ContainsKey(config.Id) == false)
                    CreateItemView(config);
            }
        }

        protected override void CreateItemView(ItemConfig config)
        {
            ItemView itemView = Instantiate(_prefabItemView, _contentParent);
            itemView.Init(config, _inventory);
            _itemViews.Add(config.Id, itemView);
        }
        
        protected override List<ItemConfig> GetUnlockedItems()
        {
            return _inventory.GetUnlockedItems();
        }
    }
}