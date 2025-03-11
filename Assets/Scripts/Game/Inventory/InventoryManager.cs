using System;
using System.Collections.Generic;
using IdleCarService.Core;
using IdleCarService.Progression;
using IdleCarService.Utils;
using Random = UnityEngine.Random;

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

        public ItemConfig GetRandomUnlockedItem()
        {
            for (int i = 0; i < _availableItems.Count; i++)
            {
                ItemConfig config = _availableItems[Random.Range(0, _availableItems.Count)];
                
                if (config.UnlockLevel >= _level.CurrentLevel)
                    return config;
            }
            
            return null;
        }

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

            if (_items[itemId] < 0)
                _items[itemId] = 0;

            if (CurrentItemQuantity < 0)
                CurrentItemQuantity = 0;
            
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

        public InventoryItemData[] GetSaveData()
        {
            InventoryItemData[] inventoryArray = new InventoryItemData[_items.Count];
            int[] keys = new int[_items.Count];
            _items.Keys.CopyTo(keys, 0);

            for (int i = 0; i < keys.Length; i++)
            {
                int key = keys[i];
                inventoryArray[i] = new InventoryItemData { Id = key, Quantity = _items[key] };
            }

            return inventoryArray;
        }

        public void LoadData(InventoryItemData[] inventoryData)
        {
            for (int i = 0; i < inventoryData.Length; i++)
            {
                InventoryItemData item = inventoryData[i];
                _items[item.Id] = item.Quantity;
                OnItemQuantityChanged?.Invoke(item.Id, item.Quantity);
            }

            UpdateCurrentItemQuantity();
            OnInventoryHasQuantity?.Invoke(CurrentItemQuantity < MaxItemQuantity);
        }

        private void UpdateCurrentItemQuantity()
        {
            CurrentItemQuantity = 0;
            
            foreach (int quantity in _items.Values)
                CurrentItemQuantity += quantity;
        }
    }
}