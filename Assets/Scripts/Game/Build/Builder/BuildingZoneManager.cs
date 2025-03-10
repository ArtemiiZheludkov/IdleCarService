using UnityEngine;

namespace IdleCarService.Build
{
   public class BuildingZoneManager : MonoBehaviour
   {
       [SerializeField] private BuildingZone _warehouseZone;
       [SerializeField] private BuildingZone _stationZone;
       [SerializeField] private BuildingZone _factoryZone;

        public void Init()
        {
            _warehouseZone.Init();
            _stationZone.Init();
            _factoryZone.Init();
        }

        public bool HasAvailableSlotInZone(BuildingZoneType zoneType) => GetZoneByType(zoneType)?.AvailableSlots > 0;
        
        public void ExpandZone(BuildingZoneType zoneType, int slotsToAdd) => GetZoneByType(zoneType)?.ExpandZone(Vector3.back, slotsToAdd);
        
        public BuildingZone GetZoneByType(BuildingZoneType zoneType)
        {
            switch (zoneType)
            {
                case BuildingZoneType.Warehouse:
                    return _warehouseZone;
                case BuildingZoneType.Station:
                    return _stationZone;
                case BuildingZoneType.Factory:
                    return _factoryZone;
                default:
                    return null;
            }
        }
   }
}