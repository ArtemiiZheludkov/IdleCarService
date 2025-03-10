using IdleCarService.Build;
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
        public BuildingManager BuildingManager { get; private set; }
        public InventoryManager InventoryManager { get; private set; }
        public CraftManager CraftManager { get; private set; }

        [SerializeField] private GameConfig _config;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private BuildingZoneManager _buildingZoneManager;

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
            MoneyBank = new MoneyBank(_config.StartMoney);
            InventoryManager = new InventoryManager(_config.ItemConfigs, _config.StartInventoryQuantity, LevelController);
            CraftManager = new CraftManager(InventoryManager);
            
            BuildingManager = new BuildingManager(_config.BuildingConfigs, _buildingZoneManager, 
                LevelController, MoneyBank, InventoryManager);
            
            _uiManager.Init(Instance);
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
               LevelController.AddExperience(50);
            }
        }

        public void CloseGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}