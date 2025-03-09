using System.Collections.Generic;
using IdleCarService.Progression;

namespace IdleCarService.Build
{
    public class BuildingManager
    {
        private LevelController _level;
        private MoneyBank _bank;
        private Dictionary<int, BuildingConfig> _buildingConfigs;

        public BuildingManager(List<BuildingConfig> buildings, LevelController level, MoneyBank bank)
        {
            _level = level;
            _bank = bank;
            
            _buildingConfigs = new Dictionary<int, BuildingConfig>();
            
            foreach (BuildingConfig config in buildings)
                _buildingConfigs.Add(config.Id, config);
        }

        public bool CanBuild(int buildId)
        {
            if (_buildingConfigs.TryGetValue(buildId, out BuildingConfig config) == false)
                return false;
            
            if (config.UnlockLevel > _level.CurrentLevel)
                return false;
            
            if (config.BuildPrice > _bank.Money)
                return false;
            
            return true;
        }

        public bool TryBuild(int buildId)
        {
            if (CanBuild(buildId) == false)
                return false;
            
            // BUILDER TRY BUILD
            
            return true;
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
    }
}