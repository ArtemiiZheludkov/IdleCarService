using IdleCarService.Craft;
using IdleCarService.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.GamePlay
{
    public class CraftItemView : MonoBehaviour
    {
        [SerializeField] private Image _mainIcon;
        [SerializeField] private Button _craftButton;
        
        [SerializeField] private Image _ingredientPrefab;
        [SerializeField] private Transform _ingredientsParent;
        
        private ItemConfig _config;
        private CraftManager _crafter;
        private InventoryManager _inventory;

        private void OnEnable()
        {
            UpdateCraftButton();
            
            _craftButton.onClick.AddListener(OnCraftClicked);
            
            _inventory.OnItemQuantityChanged += OnItemQuantityChanged;
        }

        private void OnDisable()
        {
            UpdateCraftButton();
            
            _craftButton.onClick.RemoveListener(OnCraftClicked);
            
            _inventory.OnItemQuantityChanged -= OnItemQuantityChanged;
        }

        public void Init(ItemConfig config, CraftManager crafter, InventoryManager inventory)
        {
            _config = config;
            _crafter = crafter;
            _inventory = inventory;
            
            _mainIcon.sprite = config.Icon;
            
            UpdateCraftButton();
            CreateViews(config);
        }
        
        private void CreateViews(ItemConfig config)
        {
            foreach (ItemConfig ingredient in config.CraftingIngredients)
            {
                Image icon = Instantiate(_ingredientPrefab, _ingredientsParent);
                icon.sprite = ingredient.Icon;
            }
        }

        private void UpdateCraftButton() => _craftButton.interactable = _crafter != null && _crafter.CanCraftItem(_config.Id);

        private void OnItemQuantityChanged(int id, int count)
        {
            foreach (ItemConfig ingredient in _config.CraftingIngredients)
            {
                if (ingredient.Id == id)
                {
                    UpdateCraftButton();
                    return;
                }
            }
        }

        private void OnCraftClicked()
        {
            _crafter?.TryCraftItem(_config.Id);
            UpdateCraftButton();
        }
    }
}