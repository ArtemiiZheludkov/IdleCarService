using UnityEngine;

namespace IdleCarService.Build
{
    [CreateAssetMenu(fileName = "NewStation", menuName = "Configs/Station")]
    public class StationConfig : BuildingConfig
    {
        public ServiceStation Prefab;
        public int JobTime;
        public int JobExperience;
    }
}