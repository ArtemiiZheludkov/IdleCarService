using System.Collections.Generic;
using IdleCarService.Build;
using IdleCarService.Inventory;
using UnityEngine;

namespace IdleCarService.Core
{
    [CreateAssetMenu(fileName = "NewGameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public int StartMoney;
        public int StartInventoryQuantity;
        
        public List<ItemConfig> ItemConfigs;
        public List<BuildingConfig> BuildingConfigs;
    }
}