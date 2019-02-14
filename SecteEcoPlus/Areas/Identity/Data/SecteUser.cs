using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using SecteEcoPlus.Models;

namespace SecteEcoPlus.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the SecteUser class
    public class SecteUser : IdentityUser
    {
        [PersonalData, ForeignKey(nameof(PublicProfileId))]
        public PublicProfile PublicProfile { get; set; }
        public int PublicProfileId { get; set; }
    }
}
