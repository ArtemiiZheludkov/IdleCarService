using UnityEngine;
using UnityEngine.UI;

namespace IdleCarService.Build
{
    public abstract class WorkBuilding : Building
    {
        protected bool HasJob;
        
        [SerializeField] private Image _jobIcon;
        
        private float _jobDuration, _jobTime;

        public abstract void JobCompleted();
        
        public override void Init(BuildingConfig config)
        {
            base.Init(config);
            HasJob = false;
        }

        public virtual void StartWork()
        {
            enabled = true;
        }

        public virtual void StopWork()
        {
            enabled = false;
        }

        protected void SetJob(float duration)
        {
            _jobDuration = duration;
            _jobTime = 0f;
            HasJob = true;
            
            _jobIcon.enabled = true;
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