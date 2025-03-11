using System.Collections.Generic;
using IdleCarService.Build;
using IdleCarService.Craft;
using IdleCarService.Utils;
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
        [SerializeField] private ClientManager _mainRoad;
        [SerializeField] private ClientManager[] _otherRoad;
        
        private SaveSystem _saveSystem;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            GameState = GameStateType.Load;
            
            Init();
            LoadSave();
            SetMenuState();
        }

        private void Init()
        {
            _saveSystem = new SaveSystem();
            
            LevelController = new LevelController(baseExperienceRequired:_config.BaseExperienceRequired,
                experienceMultiplier:_config.ExperienceMultiplier, maxLevel:_config.MaxLevel);
            
            MoneyBank = new MoneyBank(_config.StartMoney);
            InventoryManager = new InventoryManager(_config.ItemConfigs, _config.StartInventoryQuantity, LevelController);
            CraftManager = new CraftManager(InventoryManager);
            
            BuildingManager = new BuildingManager(_config.BuildingConfigs, _buildingZoneManager, 
                LevelController, MoneyBank, InventoryManager);
            
            _uiManager.Init(Instance);
            _camera.Init();

            _mainRoad.Init(_config.MainClientsConfig);
            
            foreach (ClientManager manager in _otherRoad)
                manager.Init(_config.OtherClientsConfig);
        }

        public void SetMenuState()
        {
            GameState = GameStateType.Menu;
            
            _uiManager.SetMenuUI();
            _camera.SetMenuCamera();
            BuildingManager.SetMenuState();
            
            _mainRoad.ToggleSpawning(false);
            
            foreach (ClientManager manager in _otherRoad)
                manager.ToggleSpawning(false);
        }

        public void SetGamePlayState()
        {
            GameState = GameStateType.Game;
            
            _uiManager.SetGamePlayUI();
            _camera.SetGamePlayCamera();
            BuildingManager.SetGamePlayState();
            
            _mainRoad.ToggleSpawning(true);
            
            foreach (ClientManager manager in _otherRoad)
                manager.ToggleSpawning(true);
        }

        public void CloseGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void LoadSave()
        {
            SaveData data = _saveSystem.LoadGame();

            if (data == null)
            {
                InventoryManager.AddItem(_config.StartItem.Id, _config.StartItemQuantity);
                BuildingManager.TryBuild(_config.StartBuild.Id);
                
                SaveGame();
                return;
            }
            
            LevelController.LoadData(data.PlayerLevel, data.PlayerExperience);
            MoneyBank.LoadData(data.PlayerMoney);
            BuildingManager.LoadData(LevelController.CurrentLevel, data.BuildingIds);
            InventoryManager.LoadData(data.Inventory);
        }

        private void SaveGame()
        {
            SaveData data = new SaveData(LevelController.CurrentLevel, LevelController.CurrentExperience, 
                MoneyBank.Money, InventoryManager.GetSaveData(), BuildingManager.GetSaveData());

            _saveSystem.SaveGame(data);
        }

        private void OnApplicationPause(bool isPaused) => SaveGame();

        private void OnApplicationQuit() => SaveGame();
    }
}