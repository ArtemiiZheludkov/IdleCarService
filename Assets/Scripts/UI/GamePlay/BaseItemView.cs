using System.Collections.Generic;
using IdleCarService.Inventory;
using IdleCarService.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.GamePlay
{
    public abstract class BaseItemView : MonoBehaviour
    {
        [SerializeField] protected Transform _contentParent;
        [SerializeField] protected Button _closeButton;
        
        protected InventoryManager _inventory;
        
        protected abstract void CreateViews();
        protected abstract void CreateItemView(ItemConfig config);
        protected abstract List<ItemConfig> GetUnlockedItems();

        public virtual void Init(InventoryManager inventoryManager, LevelController levelController)
        {
            _inventory = inventoryManager;
            
            CreateViews();
            
            levelController.LevelChanged += OnLevelChanged;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            _closeButton.onClick.AddListener(Disable);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            _closeButton.onClick.RemoveListener(Disable);
        }

        private void OnLevelChanged(int level) => CreateViews();
    }
}