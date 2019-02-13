using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SecteEcoPlus.Models;

namespace SecteEcoPlus.Areas.Identity.Data
{
    public class PublicProfile
    {
        public int PublicProfileId { get; set; }

        public string DisplayName { get; set; }

        [InverseProperty(nameof(Message.Author))]
        public ICollection<Message> ReviewMessages { get; set; }

        [JsonIgnore] // let's not show the pass on download :p
        public SecteUser SecteUser { get; set; }

        public string GetDisplayName(UserManager<SecteUser> manager = null, string fallback = "Anonyme")
        {
            return DisplayName ?? SecteUser?.UserName ?? fallback;
        }
    }
}