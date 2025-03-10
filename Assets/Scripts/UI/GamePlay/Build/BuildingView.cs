using System;
using IdleCarService.Build;
using IdleCarService.Progression;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.GamePlay
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _priceTxt;
        [SerializeField] private Button _buildButton;

        private BuildingConfig _config;
        private BuildingManager _builder;
        private MoneyBank _bank;
        private Action _closeBuildView;

        private bool _initialized = false;
        
        private void OnEnable()
        {
            if (_initialized == false)
                return;
            
            UpdateBuildButton();
            
            _buildButton.onClick.AddListener(OnBuildClicked);
            _bank.MoneyChanged += OnMoneyChanged;
        }

        private void OnDisable()
        {
            if (_initialized == false)
                return;
            
            UpdateBuildButton();
            
            _buildButton.onClick.RemoveListener(OnBuildClicked);
            _bank.MoneyChanged -= OnMoneyChanged;
        }
        
        public void Init(BuildingConfig config, BuildingManager manager, MoneyBank bank, Action closeBuildView)
        {
            _config = config;
            _builder = manager;
            _bank = bank;
            _closeBuildView = closeBuildView;
            
            _icon.sprite = config.Icon;
            _priceTxt.text = config.BuildPrice.ToString();
            _initialized = true;

            if (gameObject.activeInHierarchy) 
                OnEnable();
        }
        
        private void UpdateBuildButton() => _buildButton.interactable = _builder != null && _builder.CanBuild(_config.Id);

        private void OnMoneyChanged(int money) => UpdateBuildButton();

        private void OnBuildClicked()
        {
            _builder?.TryBuild(_config.Id);
            _closeBuildView?.Invoke();
        }
    }
}