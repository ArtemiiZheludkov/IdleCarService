namespace IdleCarService.Utils
{
    [System.Serializable]
    public class SaveData
    {
        public int PlayerLevel;
        public int PlayerExperience;
        public int PlayerMoney;
        public InventoryItemData[] Inventory;
        public int[] BuildingIds;

        public SaveData(int level, int experience, int money, InventoryItemData[] inventory, int[] buildingIds)
        {
            PlayerLevel = level;
            PlayerExperience = experience;
            PlayerMoney = money;
            Inventory = inventory;
            BuildingIds = buildingIds;
        }
    }
    
    [System.Serializable]
    public class InventoryItemData
    {
        public int Id;
        public int Quantity;
    }
}