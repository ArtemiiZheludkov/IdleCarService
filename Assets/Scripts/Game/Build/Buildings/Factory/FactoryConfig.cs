using IdleCarService.Inventory;
using UnityEngine;

namespace IdleCarService.Build
{
    [CreateAssetMenu(fileName = "NewFactory", menuName = "Configs/Factory")]
    public class FactoryConfig : BuildingConfig
    {
        public Factory Prefab;
        public ItemConfig FactoryItem;
        public int FactoryPrice;
        public int JobTime;
    }
}