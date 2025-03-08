using UnityEngine;

namespace IdleCarService.Build
{
    [CreateAssetMenu(fileName = "NewBuilding", menuName = "Configs/Building")]
    public class StationConfig : BuildingConfig
    {
        public WorkBuilding Prefab;
        public int JobTime;
    }
}