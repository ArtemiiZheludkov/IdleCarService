using System.Collections.Generic;
using IdleCarService.Build;
using IdleCarService.Inventory;
using IdleCarService.Unit;
using UnityEngine;

namespace IdleCarService.Core
{
    [CreateAssetMenu(fileName = "NewGameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public int BaseExperienceRequired;
        public float ExperienceMultiplier;
        public int MaxLevel;
        
        public int StartMoney;
        public int StartInventoryQuantity;

        public ItemConfig StartItem;
        public int StartItemQuantity;
        
        public BuildingConfig StartBuild;
        
        public ClientsConfig MainClientsConfig;
        public ClientsConfig OtherClientsConfig;
        
        public List<ItemConfig> ItemConfigs;
        public List<BuildingConfig> BuildingConfigs;
    }
}