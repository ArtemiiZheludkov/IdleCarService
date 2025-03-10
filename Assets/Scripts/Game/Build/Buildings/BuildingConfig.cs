using UnityEngine;

namespace IdleCarService.Build
{
    public abstract class BuildingConfig : ScriptableObject
    {
        public int Id;
        public Sprite Icon;
        public BuildingZoneType ZoneType;
        
        public int UnlockLevel;
        public int BuildPrice;
        
        private void OnValidate()
        {
            if (Id == 0)
                Id = Random.Range(10000, 999999);
        }
    }
}