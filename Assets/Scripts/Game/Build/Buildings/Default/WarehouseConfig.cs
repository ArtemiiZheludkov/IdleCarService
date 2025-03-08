using UnityEngine;

namespace IdleCarService.Build
{
    [CreateAssetMenu(fileName = "NewBuilding", menuName = "Configs/Building")]
    public class WarehouseConfig : BuildingConfig
    {
        public Warehouse Prefab;
        public int AddInventoryQuantity;
    }
}