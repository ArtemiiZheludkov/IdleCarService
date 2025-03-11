using UnityEngine;

namespace IdleCarService.Build
{
    public abstract class Building : MonoBehaviour
    {
        public int Id { get; private set; }

        public virtual void Init(BuildingConfig config)
        {
            Id = config.Id;
        }
    }
}