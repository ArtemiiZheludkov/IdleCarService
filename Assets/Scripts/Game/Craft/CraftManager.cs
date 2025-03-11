using System.Collections.Generic;
using IdleCarService.Inventory;
using Unity.VisualScripting;

namespace IdleCarService.Craft
{
    public class CraftManager
    {
        private InventoryManager _inventoryManager;
        
        private readonly int _requiredCount;

        public CraftManager(InventoryManager inventoryManager)
        {
            _inventoryManager = inventoryManager;

            _requiredCount = 1;
        }

        public bool HasIngredient(int itemId, int quantity) => _inventoryManager.HasItem(itemId, quantity);
        
        public bool TryCraftItem(int itemId)
        {
            ItemConfig itemConfig = _inventoryManager.GetItemConfig(itemId);
            
            if (itemConfig == null || itemConfig.IsCraftable == false)
                return false;
            
            if (CanCraftItem(itemId) == false)
                return false;
            
            foreach (ItemConfig ingredient in itemConfig.CraftingIngredients)
            {
                if (ingredient == null) 
                    return false;
                
                _inventoryManager.RemoveItem(ingredient.Id, _requiredCount);
            }

            _inventoryManager.AddItem(itemId);
            
            return true;
        }

        public bool CanCraftItem(int itemId)
        {
            if (_inventoryManager.CurrentItemQuantity >= _inventoryManager.MaxItemQuantity)
                return false;
            
            ItemConfig itemConfig = _inventoryManager.GetItemConfig(itemId);
            
            if (itemConfig == null || itemConfig.IsCraftable == false)
                return false;

            foreach (ItemConfig ingredient in itemConfig.CraftingIngredients)
            {
                if (ingredient == null) 
                    return false;
                
                if (_inventoryManager.HasItem(ingredient.Id, _requiredCount) == false)
                    return false;
            }
            
            return true;
        }

        public List<ItemConfig> GetUnlockedCraftableItems()
        {
            List<ItemConfig> craftableItems = new List<ItemConfig>();
            
            foreach (var item in _inventoryManager.GetUnlockedItems())
            {
                if (item.IsCraftable)
                    craftableItems.Add(item);
            }
            
            return craftableItems;
        }
        
        public Dictionary<int, int> GetCraftingInfo(int itemId)
        {
            Dictionary<int, int> craftingInfo = new Dictionary<int, int>();
            
            ItemConfig itemConfig = _inventoryManager.GetItemConfig(itemId);
            
            if (itemConfig == null || itemConfig.IsCraftable == false)
                return craftingInfo;
            
            foreach (ItemConfig ingredient in itemConfig.CraftingIngredients)
            {
                if (ingredient == null) 
                    continue;
                
                craftingInfo.Add(ingredient.Id, _requiredCount);
            }

            return craftingInfo;
        }
    }
}