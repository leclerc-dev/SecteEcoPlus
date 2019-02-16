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
            set
            {
                if (ExperienceDebt <= 0)
                {
                    _experience = value;
                }
                var diff = value - _experience;
                var afterDebt = ExperienceDebt - diff;
                ExperienceDebt = Math.Max(afterDebt, 0);
                _experience += Math.Max(-afterDebt, 0);
            }
        }

        public int Level => GetLevel(Experience);
        public int ExperienceNeededForNextLevel => GetExperienceNeededForNextLevel(Level);
        public int ExperienceLeftForNextLevel => GetExperienceLeftForNextLevel(Level, Experience);
        public int NextLevel => GetNextLevel(Level);
    }
}