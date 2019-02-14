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
        public async Task<IActionResult> OnGet(int? id = null, string name = null)
        {
            if (name != null && id != null) return await GetUserPage(id.Value);
            if (id is null && _signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                return RedirectToPage(new { id = user.PublicProfileId });
            }
            if (id is null)
            {
                return NotFound("Erreur :c je pleur +");
            }

            return RedirectToPage("Index", new {id, name = (await _context.PublicProfiles.FindAsync(id)).DisplayName});
        }
        private async Task<IActionResult> GetUserPage(int id)
        {
            //var user = await _userManager.FindByIdAsync(id.ToString());
            var profile = await _context.PublicProfiles
                .AsNoTracking()
                .Include(p => p.ReviewMessages)
                    .ThenInclude(m => m.Author)
                .FirstOrDefaultAsync(u => u.PublicProfileId == id);

            if (profile is null)
            {
                return NotFound("Utilisateur introuvable :( je pleur");
            }
            Profile = profile;
            ViewData["Profile"] = Profile; // no ViewBag :c 
            return Page();
        }
    }
}