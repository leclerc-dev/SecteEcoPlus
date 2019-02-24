using SecteEcoPlus.Areas.Identity.Data;
using System;

namespace SecteEcoPlus.Experience
{
    public static class ExperienceHelper
    {
        public const double LevelGrowth = 0.273;

        public static int GetLevel(int xp) => (int)(LevelGrowth * Math.Sqrt(xp)) + 1;
        public static int GetNextLevel(int lvl) => lvl + 1;
        public static int GetExperienceNeededForNextLevel(int lvl) => (int)Math.Pow(lvl / LevelGrowth, 2);
        public static int GetExperienceLeftForNextLevel(int lvl, int xp) => GetExperienceNeededForNextLevel(lvl) - xp;
        public static string GetXpTitle(this IExperienceHolder holder) => PublicProfile.GetTitle(GetLevel(holder.Experience));
        public static string GetLevelTitle(int lvl) => PublicProfile.GetTitle(GetLevel(lvl));
    }
}