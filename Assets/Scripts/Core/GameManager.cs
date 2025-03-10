using IdleCarService.Build;
using IdleCarService.Craft;
using IdleCarService.Camera;
using IdleCarService.Inventory;
using IdleCarService.Progression;
using IdleCarService.UI;
using IdleCarService.Unit;
using UnityEngine;

namespace IdleCarService.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public GameStateType GameState { get; private set; }
        
        public LevelController LevelController { get; private set; }
        public MoneyBank MoneyBank { get; private set; }
        public BuildingManager BuildingManager { get; private set; }
        public InventoryManager InventoryManager { get; private set; }
        public CraftManager CraftManager { get; private set; }

        [SerializeField] private GameConfig _config;
        [SerializeField] private CameraController _camera;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private BuildingZoneManager _buildingZoneManager;
        [SerializeField] private ClientManager[] _clientManagers;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            GameState = GameStateType.Load;
            
            Init();
            //Load save
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
            _camera.Init();

            foreach (ClientManager manager in _clientManagers)
                manager.Init(_config.ClientsConfig);
        }

        public void SetMenuState()
        {
            GameState = GameStateType.Menu;
            
            _uiManager.SetMenuUI();
            _camera.SetMenuCamera();
            BuildingManager.SetMenuState();
            
            foreach (ClientManager manager in _clientManagers)
                manager.ToggleSpawning(false);
        }

        public void SetGamePlayState()
        {
            GameState = GameStateType.Game;
            
            _uiManager.SetGamePlayUI();
            _camera.SetGamePlayCamera();
            BuildingManager.SetGamePlayState();
            
            foreach (ClientManager manager in _clientManagers)
                manager.ToggleSpawning(true);
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