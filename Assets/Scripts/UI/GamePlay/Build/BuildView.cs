using System.Collections.Generic;
using IdleCarService.Build;
using IdleCarService.Progression;
using UnityEngine;

namespace IdleCarService.UI.GamePlay
{
    public class BuildView : BaseListView
    {
        [SerializeField] private BuildingView _prefabBuildingView;
        
        private BuildingManager _buildingManager;
        private MoneyBank _bank;
        private Dictionary<int, BuildingView> _buildingViews;
        
        public void Init(BuildingManager buildingManager, MoneyBank bank, LevelController levelController)
        {
            _buildingManager = buildingManager;
            _bank = bank;
            base.Init(levelController);
        }

        protected override void CreateViews()
        {
            if (_buildingViews == null)
                _buildingViews = new Dictionary<int, BuildingView>();

            List<BuildingConfig> unlockedConfigs = _buildingManager.GetUnlockedBuildings();

            foreach (BuildingConfig config in unlockedConfigs)
            {
                if (_buildingViews.ContainsKey(config.Id) == false)
                    CreateItemView(config);
            }
        }

        private void CreateItemView(BuildingConfig config)
        {
            BuildingView itemView = Instantiate(_prefabBuildingView, _contentParent);
            itemView.Init(config, _buildingManager, _bank, Disable);
            _buildingViews.Add(config.Id, itemView);
        }
    }
}