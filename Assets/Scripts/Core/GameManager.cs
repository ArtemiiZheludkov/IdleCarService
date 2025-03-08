using System.Collections.Generic;
using IdleCarService.Craft;
using IdleCarService.Inventory;
using IdleCarService.Progression;
using IdleCarService.UI;
using IdleCarService.Utils;
using UnityEngine;

namespace IdleCarService.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public LevelController LevelController { get; private set; }
        public MoneyBank MoneyBank { get; private set; }
        public InventoryManager InventoryManager { get; private set; }
        public CraftManager CraftManager { get; private set; }

        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private UIManager _uiManager;
        
        [SerializeField] private List<ItemConfig> _itemConfigs;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            Init();
            SetMenuState();
        }

        private void Init()
        {
            LevelController = new LevelController();
            MoneyBank = new MoneyBank();
            InventoryManager = new InventoryManager(_itemConfigs);
            CraftManager = new CraftManager(InventoryManager);
            
            _uiManager.Init();
        }

        public void SetMenuState()
        {
            _uiManager.SetMenuUI();
            _cameraShaker.StartShaking();
        }

        public void SetGamePlayState()
        {
            _uiManager.SetGamePlayUI();
            _cameraShaker.StopShaking();
        }
    }
}