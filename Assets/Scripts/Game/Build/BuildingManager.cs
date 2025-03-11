using System.Collections.Generic;
using IdleCarService.Inventory;
using IdleCarService.Progression;

namespace IdleCarService.Build
{
    public class BuildingManager
    {
        private BuildingZoneManager _zoneManager;
        private LevelController _level;
        private MoneyBank _bank;
        
        private Builder _builder;
        private Dictionary<int, BuildingConfig> _buildingConfigs;
        private List<Building> _buildingsCreated;

        public BuildingManager(List<BuildingConfig> buildings, BuildingZoneManager zoneManager, 
            LevelController level, MoneyBank bank, InventoryManager inventory)
        {
            _zoneManager = zoneManager;
            _level = level;
            _bank = bank;

            zoneManager.Init();
            _builder = new Builder(zoneManager, bank, level, inventory);
            _buildingConfigs = new Dictionary<int, BuildingConfig>();
            _buildingsCreated = new List<Building>();
            
            foreach (BuildingConfig config in buildings)
                _buildingConfigs.Add(config.Id, config);
            
            level.LevelChanged += OnLevelChanged;
        }

        public void SetMenuState()
        {
            foreach (Building building in _buildingsCreated)
            {
                if (building is WorkBuilding workBuilding)
                    workBuilding.StopWork();
            }
        }

        public void SetGamePlayState()
        {
            foreach (Building building in _buildingsCreated)
            {
                if (building is WorkBuilding workBuilding)
                    workBuilding.StartWork();
            }
        }

        public bool CanBuild(int buildId)
        {
            if (_buildingConfigs.TryGetValue(buildId, out BuildingConfig config) == false)
                return false;
            
            if (config.UnlockLevel > _level.CurrentLevel)
                return false;
            
            if (config.BuildPrice > _bank.Money)
                return false;
            
            if (_zoneManager.HasAvailableSlotInZone(config.ZoneType) == false)
                return false;
            
            return true;
        }

        public bool TryBuild(int buildId)
        {
            if (CanBuild(buildId) == false)
                return false;

            if (_buildingConfigs.TryGetValue(buildId, out BuildingConfig config) == false)
                return false;

            if (_builder.TryBuild(config, out Building building))
            {
                _bank.TrySpendMoney(config.BuildPrice);
                _buildingsCreated.Add(building);
                
                return true;
            }
            
            return false;
        }

        public List<BuildingConfig> GetUnlockedBuildings()
        {
            int playerLevel = _level.CurrentLevel;

            List<BuildingConfig> unlockedBuildings = new List<BuildingConfig>();

            if (_buildingConfigs == null)
                return unlockedBuildings;

            foreach (BuildingConfig build in _buildingConfigs.Values)
            {
                if (playerLevel >= build.UnlockLevel)
                    unlockedBuildings.Add(build);
            }

            return unlockedBuildings;
        }

        public int[] GetSaveData()
        {
            int[] data = new int[_buildingsCreated.Count];
            
            for (int i = 0; i < _buildingsCreated.Count; i++)
                data[i] = _buildingsCreated[i].Id;
            
            return data;
        }

        public void LoadData(int level, int[] data)
        {
            _zoneManager.RebuildZones(level);

            foreach (int buildId in data)
            {
                if (_buildingConfigs.TryGetValue(buildId, out BuildingConfig config))
                    if (_builder.TryBuild(config, out Building building))
                        _buildingsCreated.Add(building);
            }
        }

        private void OnLevelChanged(int level)
        {
            _zoneManager.ExpandZone(BuildingZoneType.Warehouse, 1);
            _zoneManager.ExpandZone(BuildingZoneType.Station, 1);
            _zoneManager.ExpandZone(BuildingZoneType.Factory, 1);
        }
    }
}