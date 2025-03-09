using System;
using IdleCarService.Inventory;

namespace IdleCarService.Build
{
    public class RepairStation  : ServiceStation
    {
        private int[] _detailIds;
        private int _totalSellPrice;
        
        public void CreateServiceForClient(ItemConfig[] details, Action onCompleted)
        {
            OnJobCompletedCallback = onCompleted;
            
            _detailIds = new int[details.Length];
            _totalSellPrice = 0;
    
            for (int i = 0; i < details.Length; i++)
            {
                _detailIds[i] = details[i].Id;
                _totalSellPrice += details[i].SellPrice;
            }
            
            NeedItems = true;
            WaitCount = 0;
            
            TryCreateJob();
        }

        protected override bool CanCompleteService()
        {
            if (_detailIds == null)
                return false;

            foreach (int itemId in _detailIds)
            {
                if (Inventory.HasItem(itemId) == false)
                    return false;
            }
            
            return true;
        }

        protected override void ProcessItems()
        {
            foreach (int itemId in _detailIds)
                Inventory.RemoveItem(itemId);
            
            Bank.AddMoney(_totalSellPrice);
        }
        
        protected override void UpdateInfoView()
        {
            HideInfoIcon();
            
            if (_detailIds == null)
                return;

            foreach (int itemId in _detailIds)
            {
                if (Inventory.HasItem(itemId) == false)
                {
                    ShowInfoIcon(Inventory.GetItemConfig(itemId).Icon);
                    return;
                }
            }
        }

        protected override void OnItemQuantityChanged(int id, int quantity)
        {
            if (NeedItems)
            {
                foreach (int itemId in _detailIds)
                {
                    if (itemId == id)
                    {
                        TryCreateJob();
                        break;
                    }
                }
            }
        }
    }
}