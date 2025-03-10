using IdleCarService.Inventory;
using IdleCarService.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.Build
{
    public class Factory : WorkBuilding
    {
        [SerializeField] private Sprite _moneySprite;
        [SerializeField] private Image _itemIcon;
        
        private InventoryManager _inventory;
        private MoneyBank _bank;
        private int _itemId, _itemPrice, _createTime;
        
        private bool _needAddItemToInventory;
        
        public void Init(FactoryConfig config, InventoryManager inventory, MoneyBank bank)
        {
            base.Init(config);
            
            _inventory = inventory;
            _bank = bank;
            _itemId = config.FactoryItem.Id;
            _itemPrice = config.FactoryPrice;
            _createTime = config.JobTime;
            _itemIcon.sprite = config.FactoryItem.Icon;

            _needAddItemToInventory = false;
        }

        public override void JobCompleted()
        {
            _needAddItemToInventory = !_inventory.AddItem(_itemId);

            if (_needAddItemToInventory)
                _inventory.OnInventoryHasQuantity += OnInventoryQuantityChanged;
            else
                TryCreateJob();
        }

        public override void StartWork()
        {
            base.StartWork();
            
            if (_needAddItemToInventory)
                _inventory.OnInventoryHasQuantity += OnInventoryQuantityChanged;
            else if (HasJob == false)
                TryCreateJob();
        }

        public override void StopWork()
        {
            base.StopWork();
            
            _bank.MoneyChanged -= OnMoneyChanged;
            _inventory.OnInventoryHasQuantity -= OnInventoryQuantityChanged;
        }

        private void TryCreateJob()
        {
            _bank.MoneyChanged -= OnMoneyChanged;
            
            if (_bank.TrySpendMoney(_itemPrice))
            {
                SetJob(_createTime);
                HideInfoIcon();
            }
            else
            {
                ShowInfoIcon(_moneySprite);
                _bank.MoneyChanged += OnMoneyChanged;
            }
        }
        
        private void OnMoneyChanged(int money)
        {
            if (HasJob)
                _bank.MoneyChanged -= OnMoneyChanged;
            else if (money >= _itemPrice)
                TryCreateJob();
        }
        
        private void OnInventoryQuantityChanged(bool hasQuantity)
        {
            if (hasQuantity == false)
                return;
            
            if (_inventory.AddItem(_itemId))
            {
                _needAddItemToInventory = false;
                _inventory.OnInventoryHasQuantity -= OnInventoryQuantityChanged;
                TryCreateJob();
            }
        }
    }
}