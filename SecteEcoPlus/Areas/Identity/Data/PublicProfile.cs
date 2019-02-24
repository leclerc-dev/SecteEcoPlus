using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SecteEcoPlus.Experience;
using SecteEcoPlus.Models;
using SecteEcoPlus.ViewModels;

namespace SecteEcoPlus.Areas.Identity.Data
{
    public class PublicProfile : ExperienceObject, IPublicProfileDisplay
    {
        public static readonly PublicProfile NotFoundProfile = new PublicProfile
        {
            DisplayName = "Introuvable",
            ReviewMessages = Array.Empty<Message>()        
        };

        [NotMapped]
        int IPublicProfileDisplay.Id
        {
            get => PublicProfileId;
            set => PublicProfileId = value;
        }

        public int PublicProfileId { get; set; }
        [StringLength(64)]
        [Display(Name = "Pseudonyme", ShortName = "Pseudo")]
        [DisplayFormat(NullDisplayText = "Anonyme")]
        public string DisplayName { get; set; }

        [InverseProperty(nameof(Message.Author))]
        public ICollection<Message> ReviewMessages { get; set; }

        public ICollection<ProductIdea> ProductIdeas { get; set; }

        // ---
        [JsonIgnore] // let's not show the pass on download :p
        public SecteUser SecteUser { get; set; }

        public string GetDisplayName(string fallback = "Anonyme")
        {
            return DisplayName ?? SecteUser?.UserName ?? fallback;
        }

        public static string GetTitle(int lvl)
        {
            return LevelTitles.Count < lvl ? LevelTitles.Last() : LevelTitles[lvl - 1];
        }

        public string GetTitle() => GetTitle(Level);
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