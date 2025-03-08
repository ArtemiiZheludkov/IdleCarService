using IdleCarService.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.GamePlay
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countTxt;

        public void Init(ItemConfig config, InventoryManager inventory)
        {
            _icon.sprite = config.Icon;
            _countTxt.text = inventory.GetItemQuantity(config.Id).ToString();
            inventory.OnItemQuantityChanged += OnItemQuantityChanged;
        }

        private void OnItemQuantityChanged(int id, int count)
        {
            _countTxt.text = count.ToString();
        }
    }
}