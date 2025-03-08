using System.Collections.Generic;
using IdleCarService.Craft;
using IdleCarService.Inventory;
using IdleCarService.Progression;
using UnityEngine;

namespace IdleCarService.UI.GamePlay
{
    public class CraftView : BaseItemView
    {
        [SerializeField] private CraftItemView _prefabItemView;
        
        private CraftManager _craft;
        private InventoryManager _inventory;
        private Dictionary<int, CraftItemView> _itemViews;
        
        public void Init(CraftManager craftManager, InventoryManager inventory, LevelController levelController)
        {
            _craft = craftManager;
            _inventory = inventory;
            base.Init(levelController);
        }

        protected override void CreateViews()
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

        protected override void CreateItemView(ItemConfig config)
        {
            CraftItemView itemView = Instantiate(_prefabItemView, _contentParent);
            itemView.Init(config, _craft, _inventory);
            _itemViews.Add(config.Id, itemView);
        }
    }
}