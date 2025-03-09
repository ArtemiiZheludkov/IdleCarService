using System;
using System.Collections.Generic;
using IdleCarService.Core;
using IdleCarService.Progression;

namespace IdleCarService.Inventory
{
    public class InventoryManager
    {
        public event Action<int, int> OnItemQuantityChanged;
        public event Action<bool> OnInventoryHasQuantity;
        
        public int CurrentItemQuantity { get; private set; }
        public int MaxItemQuantity { get; private set; }

        private LevelController _level;
        private List<ItemConfig> _availableItems;
        private Dictionary<int, int> _items;

        public InventoryManager(List<ItemConfig> availableItems, int maxItemQuantity, LevelController level)
        {
            _availableItems = availableItems;
            MaxItemQuantity = maxItemQuantity;
            _level = level;

            CurrentItemQuantity = 0;
            _items = new Dictionary<int, int>();

            foreach (var pair in _availableItems)
                _items[pair.Id] = 0;
        }

        public bool HasItem(int itemId, int quantity = 1) => _items.ContainsKey(itemId) && _items[itemId] >= quantity;

        public int GetItemQuantity(int itemId) => _items.ContainsKey(itemId) ? _items[itemId] : 0;

        public int GetRandomItemId() => _availableItems[UnityEngine.Random.Range(0, _availableItems.Count)].Id;

        public void AddItemQuantity(int quantity)
        {
            if (quantity <= 0)
                return;
            
            MaxItemQuantity += quantity;
            OnInventoryHasQuantity?.Invoke(CurrentItemQuantity < MaxItemQuantity);
        }

        public bool AddItem(int itemId, int quantity = 1)
        {
            if (quantity <= 0 || _items.ContainsKey(itemId) == false)
                return false;

            if (CurrentItemQuantity + quantity > MaxItemQuantity)
                return false;
            
            _items[itemId] += quantity;
            CurrentItemQuantity += quantity;
            
            OnItemQuantityChanged?.Invoke(itemId, _items[itemId]);
            OnInventoryHasQuantity?.Invoke(CurrentItemQuantity < MaxItemQuantity);
            
            return true;
        }

        public bool RemoveItem(int itemId, int quantity = 1)
        {
            if (quantity <= 0 || _items.ContainsKey(itemId) == false || _items[itemId] < quantity)
                return false;

            _items[itemId] -= quantity;
            CurrentItemQuantity -= quantity;

            if (_items[itemId] <= 0)
                _items[itemId] = 0;
            
            OnItemQuantityChanged?.Invoke(itemId, _items[itemId]);
            OnInventoryHasQuantity?.Invoke(CurrentItemQuantity < MaxItemQuantity);
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
            int playerLevel = _level.CurrentLevel;

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

        public void LoadFromData(Dictionary<int, int> savedData, int maxItemQuantity)
        {
            //_items.Clear();
            
            MaxItemQuantity = maxItemQuantity;

            foreach (var pair in savedData)
            {
                _items[pair.Key] = pair.Value;
                CurrentItemQuantity += pair.Value;
                OnItemQuantityChanged?.Invoke(pair.Key, pair.Value);
            }
            
            OnInventoryHasQuantity?.Invoke(CurrentItemQuantity < MaxItemQuantity);
        }
    }
}