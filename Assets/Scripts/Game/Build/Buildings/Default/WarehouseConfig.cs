using UnityEngine;

namespace IdleCarService.Build
{
    [CreateAssetMenu(fileName = "NewWarehouse", menuName = "Configs/Warehouse")]
    public class WarehouseConfig : BuildingConfig
    {
        public Warehouse Prefab;
        public int AddInventoryQuantity;
    }
}