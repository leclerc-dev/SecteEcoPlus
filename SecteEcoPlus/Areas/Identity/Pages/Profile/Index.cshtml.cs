using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecteEcoPlus.Areas.Identity.Data;
using SecteEcoPlus.Models;
using SecteEcoPlus.Utilities;

namespace SecteEcoPlus.Areas.Identity.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly WebsiteContext _context;
        private readonly SecteUserManager _userManager;
        private readonly SignInManager<SecteUser> _signInManager;
        public IndexModel(WebsiteContext context, SecteUserManager userManager, SignInManager<SecteUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public PublicProfile Profile { get; set; }
        public IReadOnlyList<Message> Reviews { get; set; }
        public async Task<IActionResult> OnGet(int? id = null, string name = null)
        {
            if (name != null && id != null) return await GetUserPage(id.Value);
            if (id is null && _signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                return RedirectToPage(new { id = user.PublicProfileId });
            }
            PublicProfile profile;
            if (id is null || (profile = await _context.PublicProfiles.FindAsync(id)) is null)
            {           
                ViewBag.Profile = Profile = PublicProfile.NotFoundProfile;
                return Page();
            }
            
            return RedirectToPage("Index", new {id, name = profile.DisplayName.AdaptToRoute()});
        }
        private async Task<IActionResult> GetUserPage(int id)
        {
            //var user = await _userManager.FindByIdAsync(id.ToString());
            var data = await _context.PublicProfiles
                              .AsNoTracking()
                              .Select(p => new 
                              {
                                  Profile = p,
                                  Messages = p.ReviewMessages.OrderByDescending(r => r.PublishDate).Take(10).ToArray()
                              })
                              .FirstOrDefaultAsync(u => u.Profile.PublicProfileId == id);
            if (data?.Profile is null)
            {
                data = new { Profile = PublicProfile.NotFoundProfile, Messages = Array.Empty<Message>() };
            }
            Profile = data.Profile;
            Reviews = data.Messages ?? Array.Empty<Message>();
            if (Reviews.Any())
            {
                foreach (var r in Reviews)
                {
                    r.Author = Profile;
                    r.AuthorId = Profile.PublicProfileId;
                }
            }
            ViewBag.Profile = Profile; // hehe view baggie
            return Page();
        }
    }
}