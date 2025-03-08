using System;
using System.Collections.Generic;
using IdleCarService.Core;

namespace IdleCarService.Inventory
{
    public class InventoryManager
    {
        public event Action<int, int> OnItemQuantityChanged;
        
        private List<ItemConfig> _availableItems;
        private Dictionary<int, int> _items;
        
        public InventoryManager(List<ItemConfig> availableItems)
        {
            _availableItems = availableItems;
            _items = new Dictionary<int, int>();
            
            foreach (var pair in _availableItems)
                _items[pair.Id] = 0;
        }
        
        public bool HasItem(int itemId, int quantity = 1) => _items.ContainsKey(itemId) && _items[itemId] >= quantity;
        
        public int GetItemQuantity(int itemId) => _items.ContainsKey(itemId) ? _items[itemId] : 0;
        
        public bool AddItem(int itemId, int quantity = 1)
        {
            if (quantity <= 0 || _items.ContainsKey(itemId) == false)
                return false;
            
            _items[itemId] += quantity;
            OnItemQuantityChanged?.Invoke(itemId, _items[itemId]);
            return true;
        }
        
        public bool RemoveItem(int itemId, int quantity = 1)
        {
            if (quantity <= 0 || _items.ContainsKey(itemId) == false || _items[itemId] < quantity)
                return false;
            
            _items[itemId] -= quantity;
            
            if (_items[itemId] <= 0)
                _items[itemId] = 0;
            
            OnItemQuantityChanged?.Invoke(itemId, _items[itemId]);
            return true;
        }
        
        public ItemConfig GetItemConfig(int itemId)
        {
            if (_availableItems == null)
                return null;
            
            foreach (var item in _availableItems)
            {
                if (item.Id == itemId)
                    return item;
            }
            
            return null;
        }
        
        public List<ItemConfig> GetUnlockedItems()
        {
            int playerLevel = GameManager.Instance.LevelController.CurrentLevel;
            
            List<ItemConfig> unlockedItems = new List<ItemConfig>();
            
            if (_availableItems == null)
                return unlockedItems;
            
            foreach (var item in _availableItems)
            {
                if (playerLevel >= item.UnlockLevel)
                    unlockedItems.Add(item);
            }
            
            return unlockedItems;
        }
        
        public void LoadFromData(Dictionary<int, int> savedData)
        {
            //_items.Clear();
            
            foreach (var pair in savedData)
            {
                _items[pair.Key] = pair.Value;
                OnItemQuantityChanged?.Invoke(pair.Key, pair.Value);
            }
        }
    }
}
