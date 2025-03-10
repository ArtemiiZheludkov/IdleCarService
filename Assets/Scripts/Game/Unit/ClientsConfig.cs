using UnityEngine;

namespace IdleCarService.Unit
{
    [CreateAssetMenu(fileName = "NewClients", menuName = "Configs/ClientsConfig")]
    public class ClientsConfig : ScriptableObject
    {
        public int StartPool;
        public int SpawnInterval;
        public int MaxClientsOnRoad;
        
        public int MinSpeed;
        public int MaxSpeed;
        public int RotateSpeed;
        
        public Client[] Prefabs;
    }
}