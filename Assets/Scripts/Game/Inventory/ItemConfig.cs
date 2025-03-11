using UnityEngine;

namespace IdleCarService.Inventory
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Configs/Item")]
    public class ItemConfig : ScriptableObject
    {
        public int Id;
        public Sprite Icon;
        
        public int UnlockLevel;
        public int SellPrice;
        
        public bool IsCraftable = false;
        public ItemConfig[] CraftingIngredients;
        
        private void OnValidate()
        {
            if (Id == 0)
                Id = Random.Range(10000, 999999);
        }
    }
}