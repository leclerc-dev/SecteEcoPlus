using System;
using System.Collections.Generic;
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
        public IEnumerable<Message> LastMessages { get; set; }
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; }

        public async Task<IActionResult> OnGet(Guid? id = null)
        {
            if (id is null && _signInManager.IsSignedIn(User))
            {
                var subId = _userManager.GetUserId(User);
                if (string.IsNullOrWhiteSpace(subId))
                {
                    return NotFound("Une erreur très bizarre s'est produite.");
                }

                return RedirectToPage(new { id = Guid.Parse(subId) });
            }
            if (id is null)
            {
                return NotFound("Vous n'êtes pas connecté.");
            }

            return await GetUserPage(id);
        }

        private async Task<IActionResult> GetUserPage(Guid? id)
        {
            //var user = await _userManager.FindByIdAsync(id.ToString());
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.PublicProfile)
                .ThenInclude(p => p.ReviewMessages)
                .Where(u => u.Id == id.ToString())
                .Select(u => new
                {
                    u.PublicProfile.DisplayName,
                    u.PublicProfile.ReviewMessages
                }) // yes anonymous pls
                .FirstOrDefaultAsync();
            if (user is null)
            {
                return NotFound("Utilisateur introuvable :(");
            }

            Username = user.DisplayName;
            var l = user.ReviewMessages
                .Take(5)
                .OrderByDescending(m => m.PublishDate)
                .ToList();
            l.ForEach(m => m.Author = new PublicProfile { DisplayName = user.DisplayName });
            LastMessages = l;
            return Page();
        }
    }
}