using IdleCarService.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.UI.GamePlay
{
    public abstract class BaseListView : MonoBehaviour
    {
        [SerializeField] protected Transform _contentParent;
        [SerializeField] protected Button _closeButton;
        
        protected abstract void CreateViews();

        public virtual void Init(LevelController levelController)
        {
            CreateViews();
            
            levelController.LevelChanged += OnLevelChanged;
        }

        public virtual void Enable()
        {
            gameObject.SetActive(true);
            _closeButton.onClick.AddListener(Disable);
        }

        public virtual void Disable()
        {
            gameObject.SetActive(false);
            _closeButton.onClick.RemoveListener(Disable);
        }

        private void OnLevelChanged(int level) => CreateViews();
    }
}