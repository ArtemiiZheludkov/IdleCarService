using System;
using IdleCarService.Inventory;
using IdleCarService.Progression;

namespace IdleCarService.Build
{
    public abstract class ServiceStation : WorkBuilding
    {
        protected InventoryManager Inventory;
        protected MoneyBank Bank;
        protected int JobTime;
        
        protected Action OnJobCompletedCallback;
        protected bool NeedItems;
        protected int WaitCount;

        public void Init(StationConfig config, InventoryManager inventory, MoneyBank bank)
        {
            base.Init(config);
            
            Inventory = inventory;
            Bank = bank;
            JobTime = config.JobTime;
            
            NeedItems = false;
            WaitCount = 0;
        }
        
        public override void JobCompleted()
        {
            if (NeedItems && WaitCount < 2)
            {
                TryCreateJob();
                return;
            }

            NeedItems = false;
            WaitCount = 0;
            
            OnJobCompletedCallback?.Invoke();
        }

        public override void StartWork()
        {
            base.StartWork();
            
            if (NeedItems)
                Inventory.OnItemQuantityChanged += OnItemQuantityChanged;
        }

        public override void StopWork()
        {
            base.StopWork();
            Inventory.OnItemQuantityChanged -= OnItemQuantityChanged;
        }
        
        protected abstract bool CanCompleteService();
        protected abstract void ProcessItems();
        protected abstract void UpdateInfoView();
        
        protected void TryCreateJob()
        {
            Inventory.OnItemQuantityChanged -= OnItemQuantityChanged;

            bool canComplete = CanCompleteService();
            NeedItems = !canComplete;
            
            if (canComplete)
            {
                ProcessItems();
                SetJob(JobTime);
                ShowTimerView();
            }
            else
            {
                SetJob(JobTime);
                HideTimerView();
                UpdateInfoView();
                WaitCount += 1;
                Inventory.OnItemQuantityChanged += OnItemQuantityChanged;
            }
        }
        
        protected abstract void OnItemQuantityChanged(int id, int quantity);
    }
}
