using IdleCarService.Inventory;

namespace IdleCarService.Build
{
    public class Warehouse : Building
    {
        public void Init(WarehouseConfig config, InventoryManager inventory)
        {
            base.Init(config);
            inventory.AddItemQuantity(config.AddInventoryQuantity);
        }
    }
}