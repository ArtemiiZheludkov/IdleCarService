using IdleCarService.Core;
using IdleCarService.Inventory;
using IdleCarService.Progression;
using UnityEngine;

namespace IdleCarService.Build
{
    public class Builder
    {
        private BuildingZoneManager _zoneManager;
        private MoneyBank _bank;
        private InventoryManager _inventory;

        public Builder(BuildingZoneManager zoneManager, MoneyBank bank, InventoryManager inventory)
        {
            _zoneManager = zoneManager;
            _bank = bank;
            _inventory = inventory;
        }

        public bool TryBuild(BuildingConfig config, out Building newBuilding)
        {
            newBuilding = null;
            
            if (_zoneManager.HasAvailableSlotInZone(config.ZoneType) == false)
                return false;
                
            BuildingZone zone = _zoneManager.GetZoneByType(config.ZoneType);

            if (zone.CanPlaceBuilding(out Transform buildingSlot))
            {
                newBuilding = InstantiateBuilding(config, buildingSlot);
                
                if (newBuilding == null)
                    return false;
                
                zone.AddBuilding(newBuilding, buildingSlot);
                return true;
            }
            
            return false;
        }
        
        private Building InstantiateBuilding(BuildingConfig config, Transform buildingSlot)
        {
            if (config is WarehouseConfig warehouseConfig)
            {
                Warehouse warehouse = Object.Instantiate(warehouseConfig.Prefab, buildingSlot);
                warehouse.Init(warehouseConfig, _inventory);
                return warehouse;
            }
            
            if (config is StationConfig stationConfig)
            {
                ServiceStation station = Object.Instantiate(stationConfig.Prefab, buildingSlot);
                station.Init(stationConfig, _inventory, _bank);
                
                if (GameManager.Instance.GameState == GameStateType.Game)
                    station.StartWork();
                else if (GameManager.Instance.GameState == GameStateType.Menu)
                    station.StopWork();
                
                return station;
            }
            
            if (config is FactoryConfig factoryConfig)
            {
                Factory factory = Object.Instantiate(factoryConfig.Prefab, buildingSlot);
                factory.Init(factoryConfig, _inventory, _bank);
                
                if (GameManager.Instance.GameState == GameStateType.Game)
                    factory.StartWork();
                else if (GameManager.Instance.GameState == GameStateType.Menu)
                    factory.StopWork();
                
                return factory;
            }
            
            return null;
        }
    }
}