using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SecteEcoPlus.Models;

namespace SecteEcoPlus.Areas.Identity.Data
{
    public class PublicProfile
    {
        public int PublicProfileId { get; set; }

        public string DisplayName { get; set; }

        public int Experience { get; set; }

        public const double LevelGrowth = 0.2;
        public int Level => (int)(LevelGrowth * Math.Sqrt(Experience)) + 1;
        public int NextLevel => Level + 1;
        public int ExperienceNeededForNextLevel => (int)Math.Pow(Level / LevelGrowth, 2);
        public int ExperienceLeftForNextLevel => ExperienceNeededForNextLevel - Experience;

        [InverseProperty(nameof(Message.Author))]
        public ICollection<Message> ReviewMessages { get; set; }

        // ---
        [JsonIgnore] // let's not show the pass on download :p
        public SecteUser SecteUser { get; set; }

        public string GetDisplayName(UserManager<SecteUser> manager = null, string fallback = "Anonyme")
        {
            return DisplayName ?? SecteUser?.UserName ?? fallback;
        }

        public string GetTitle()
        {
            return LevelTitles.Count < Level ? LevelTitles.Last() : LevelTitles[Level - 1];
        }
        public static readonly IReadOnlyList<string> LevelTitles = new[]
        {
            "Nouveau de la secte", // 1
            "Apprenti de la secte", // 2 
            "Apprenti confirmé de la secte", // ... ok you get it
            "Leclerien qualité éco+",
            "Leclerien confirmé",
            "Leclerien avancé",
            "Leclerien quasi-expert",
            "Leclerien expert",
            "Leclerien expert suprême",
            "Leclerien expert éco+",
            "Leclerien divin expert",
            "Leclerien expert du gange",
            "Leclerien dieu du gange",
            "Leclerien dieu expert du gange",
            "Leclerien dieu divin du gange",
            "Leclerien dieu divin expert du gange",
            "Leclerien ULTIME"
        };
    }
}