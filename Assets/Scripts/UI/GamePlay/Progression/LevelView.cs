using IdleCarService.Progression;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.GamePlay
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private Slider _slider;
        
        private LevelController _levelController;

        public void Init(LevelController levelController)
        {
            _levelController = levelController;
            
            _valueText.text = _levelController.CurrentLevel.ToString();
            _slider.value = _levelController.CurrentExperience;
            _slider.maxValue = _levelController.NeedExperience;
        }

        public void Enable()
        {
            _valueText.text = _levelController.CurrentLevel.ToString();
            _slider.value = _levelController.CurrentExperience;
            _slider.maxValue = _levelController.NeedExperience;
            
            _levelController.LevelChanged += OnLevelChanged;
            _levelController.ExperienceChanged += OnExperienceChanged;
        }

        public void Disable()
        {
            _levelController.LevelChanged -= OnLevelChanged;
            _levelController.ExperienceChanged -= OnExperienceChanged;
        }
        
        private void OnExperienceChanged(int experience)
        {
            _slider.value = experience;
        }

        private void OnLevelChanged(int level)
        {
            _valueText.text = _levelController.CurrentLevel.ToString();
            _slider.value = _levelController.CurrentExperience;
            _slider.maxValue = _levelController.NeedExperience;
        }
    }
}