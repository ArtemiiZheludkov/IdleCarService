using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.Build
{
    public abstract class WorkBuilding : Building
    {
        protected bool HasJob;

        [SerializeField] private GameObject _timerView;
        [SerializeField] private GameObject _infoView;
        
        [SerializeField] private Image _jobIcon;
        [SerializeField] private Image _infoIcon;
        
        private float _jobDuration, _jobTime;

        public abstract void JobCompleted();
        
        public override void Init(BuildingConfig config)
        {
            base.Init(config);
            HasJob = false;
            
            _timerView.gameObject.SetActive(false);
            _infoView.gameObject.SetActive(false);
        }

        public virtual void StartWork()
        {
            enabled = true;
        }

        public virtual void StopWork()
        {
            enabled = false;
        }
        
        protected void ShowTimerView() => _timerView.gameObject.SetActive(true);
        
        protected void HideTimerView() => _timerView.gameObject.SetActive(false);

        protected void ShowInfoIcon(Sprite sprite)
        {
            _infoIcon.sprite = sprite;
            _infoView.gameObject.SetActive(true);
        }
        
        protected void HideInfoIcon() => _infoView.gameObject.SetActive(false);

        protected void SetJob(float duration)
        {
            _jobDuration = duration;
            _jobTime = 0f;
            HasJob = true;
            
            _jobIcon.fillAmount = 0f;
        }

        private void FixedUpdate()
        {
            if (HasJob)
                UpdateJobStatus();
        }

        private void UpdateJobStatus()
        {
            if (HasJob)
                return;
            
            _jobTime += Time.fixedDeltaTime;

            if (_jobTime >= _jobDuration)
            {
                HasJob = false;
                _jobIcon.enabled = false;
                
                JobCompleted();
            }
            else
            {
                _jobIcon.fillAmount = _jobTime / _jobDuration;
            }
        }
    }
}