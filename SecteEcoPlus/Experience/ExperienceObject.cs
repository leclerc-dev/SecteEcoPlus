using System;
using static SecteEcoPlus.Experience.ExperienceHelper;
namespace SecteEcoPlus.Experience
{
    public class ExperienceObject : IExperienceHolder
    {
        public int ExperienceDebt { get; set; }
        private int _experience;

        public int Experience
        {
            get => _experience;
            set => SetExperience(value);
        }

        public int SetExperience(int value, bool protectXp = true)
        {
            if (ExperienceDebt <= 0)
            {
                var experienceDiff = value - _experience;
                _experience = value;
                return experienceDiff;
            }
            var diff = value - _experience;
            if (protectXp && diff < 0)
            {
                ExperienceDebt -= diff;
                return 0;
            }
            var afterDebt = ExperienceDebt - diff;
            ExperienceDebt = Math.Max(afterDebt, 0);
            var modifiedExperience = Math.Max(-afterDebt, Math.Min(diff, 0));
            _experience += modifiedExperience;
            if (_experience < 0) _experience = 0;
            return modifiedExperience;
        }

        public int AddExperience(int value) => Experience += value;
        public int RemoveExperience(int value) => SetExperience(Math.Max(Experience - value, 0), false);
        public int RemoveExperienceSoft(int value) => SetExperience(Experience - value);
        public int Level => GetLevel(Experience);
        public int ExperienceNeededForNextLevel => GetExperienceNeededForNextLevel(Level);
        public int ExperienceLeftForNextLevel => GetExperienceLeftForNextLevel(Level, Experience);
        public int NextLevel => GetNextLevel(Level);
    }
}