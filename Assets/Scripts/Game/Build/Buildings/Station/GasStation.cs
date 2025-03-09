using System;
using IdleCarService.Inventory;
using UnityEngine;

namespace IdleCarService.Build
{
    public class GasStation : ServiceStation
    {
        [SerializeField] private ItemConfig _gasConfig;
        
        public void CreateServiceForClient(Action onCompleted)
        {
            OnJobCompletedCallback = onCompleted;
            NeedItems = true;
            WaitCount = 0;
            
            TryCreateJob();
        }

        protected override bool CanCompleteService()
        {
            return Inventory.HasItem(_gasConfig.Id);
        }

        protected override void ProcessItems()
        {
            Inventory.RemoveItem(_gasConfig.Id);
            Bank.AddMoney(_gasConfig.SellPrice);
        }

        protected override void UpdateInfoView()
        {
            if (CanCompleteService())
                ShowInfoIcon(_gasConfig.Icon);
            else
                HideInfoIcon();
        }

        protected override void OnItemQuantityChanged(int id, int quantity)
        {
            if (_gasConfig.Id == id && NeedItems)
                TryCreateJob();
        }
    }
}