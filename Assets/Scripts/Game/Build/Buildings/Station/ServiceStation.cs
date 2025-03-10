using System;
using IdleCarService.Unit;
using IdleCarService.Inventory;
using IdleCarService.Progression;
using UnityEngine;

namespace IdleCarService.Build
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class ServiceStation : WorkBuilding
    {
        public bool HasClient { get; private set; }
        public Transform[] EnterPath => _enterPath;
        public Transform[] ExitPath => _exitPath;

        [SerializeField] private Transform[] _enterPath;
        [SerializeField] private Transform[] _exitPath;
        
        protected InventoryManager Inventory;
        protected MoneyBank Bank;
        protected bool NeedItems;
        protected int WaitCount;
        
        private int _myJobTime;
        private Action _onJobCompleted;

        public void Init(StationConfig config, InventoryManager inventory, MoneyBank bank)
        {
            base.Init(config);
            
            Inventory = inventory;
            Bank = bank;
            _myJobTime = config.JobTime;
            
            NeedItems = false;
            WaitCount = 0;
            HasClient = false;
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
            
            HasClient = false;
            _onJobCompleted?.Invoke();
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

        public virtual void CreateServiceForClient(Action onCompleted)
        {
            _onJobCompleted = onCompleted;
        }

        protected void TryCreateJob()
        {
            Inventory.OnItemQuantityChanged -= OnItemQuantityChanged;

            bool canComplete = CanCompleteService();
            NeedItems = !canComplete;
            
            if (canComplete)
            {
                ProcessItems();
                SetJob(_myJobTime);
                ShowTimerView();
                Debug.Log("show");
            }
            else
            {
                SetJob(_myJobTime);
                HideTimerView();
                UpdateInfoView();
                WaitCount += 1;
                Inventory.OnItemQuantityChanged += OnItemQuantityChanged;
            }
        }
        
        protected abstract void OnItemQuantityChanged(int id, int quantity);
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Client client))
            {
                if (HasClient == false)
                    HasClient = client.EnterService(this);
            }
        }
    }
}
