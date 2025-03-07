using System;

namespace IdleCarService.Progression
{
    public class LevelController
    {
        public event Action<int> LevelChanged;
        public event Action<int> ExperienceChanged;
        
        public int CurrentLevel { get; private set; }
        public int CurrentExperience { get; private set; }
        public int NeedExperience { get; private set; }
        
        private readonly int _baseExperienceRequired;
        private readonly float _experienceMultiplier;
        private readonly int _maxLevel;
        
        public LevelController(int initialLevel = 1, int initialExperience = 0, int baseExperienceRequired = 100, 
            float experienceMultiplier = 1.5f, int maxLevel = 100)
        {
            _baseExperienceRequired = baseExperienceRequired;
            _experienceMultiplier = experienceMultiplier;
            _maxLevel = maxLevel;
            
            CurrentLevel = initialLevel;
            CurrentExperience = initialExperience;
            NeedExperience = CalculateRequiredExperience(CurrentLevel);
        }
        
        public void AddExperience(int amount)
        {
            if (amount <= 0 || CurrentLevel >= _maxLevel)
                return;
            
            CurrentExperience += amount;
            ExperienceChanged?.Invoke(CurrentExperience);
            
            CheckForLevelUp();
        }
        
        private int CalculateRequiredExperience(int level)
        {
            return (int)(_baseExperienceRequired * Math.Pow(_experienceMultiplier, level - 1));
        }
        
        private void CheckForLevelUp()
        {
            while (CurrentExperience >= NeedExperience && CurrentLevel < _maxLevel)
            {
                CurrentLevel++;
                CurrentExperience -= NeedExperience;
                NeedExperience = CalculateRequiredExperience(CurrentLevel);
                LevelChanged?.Invoke(CurrentLevel);
            }
            
            if (CurrentLevel >= _maxLevel)
            {
                CurrentLevel = _maxLevel;
                CurrentExperience = 0;
                LevelChanged?.Invoke(CurrentLevel);
            }
        }
        
        public void SetLevel(int level)
        {
            if (level <= 0 || level > _maxLevel)
                return;
            
            CurrentLevel = level;
            CurrentExperience = 0;
            NeedExperience = CalculateRequiredExperience(CurrentLevel);
            
            LevelChanged?.Invoke(CurrentLevel);
            ExperienceChanged?.Invoke(CurrentExperience);
        }
        
        public float GetLevelProgress()
        {
            if (CurrentLevel >= _maxLevel)
                return 1f;
            
            return (float)CurrentExperience / NeedExperience;
        }
        
        public int GetExperienceToNextLevel()
        {
            return NeedExperience - CurrentExperience;
        }
        
        public void ResetProgression(int level = 1)
        {
            SetLevel(level);
        }
    }
}