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

        public BuildingManager(List<BuildingConfig> buildings, BuildingZoneManager zoneManager, 
            LevelController level, MoneyBank bank, InventoryManager inventory)
        {
            _zoneManager = zoneManager;
            _level = level;
            _bank = bank;

            zoneManager.Init();
            _builder = new Builder(zoneManager, bank, inventory);
            _buildingConfigs = new Dictionary<int, BuildingConfig>();
            
            foreach (BuildingConfig config in buildings)
                _buildingConfigs.Add(config.Id, config);
            
            level.LevelChanged += OnLevelChanged;
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

            if (_builder.TryBuild(config))
            {
                _bank.TrySpendMoney(config.BuildPrice);
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

        private void OnLevelChanged(int level)
        {
            _zoneManager.ExpandZone(BuildingZoneType.Warehouse, 1);
            _zoneManager.ExpandZone(BuildingZoneType.Station, 1);
            _zoneManager.ExpandZone(BuildingZoneType.Factory, 1);
        }
    }
}