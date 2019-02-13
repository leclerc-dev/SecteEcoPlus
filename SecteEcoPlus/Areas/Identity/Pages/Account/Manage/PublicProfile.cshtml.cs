using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecteEcoPlus.Areas.Identity.Data;

namespace SecteEcoPlus.Areas.Identity.Pages.Account.Manage
{
    public class PublicProfileModel : PageModel
    {
        private SecteUserManager _userManager;

        public PublicProfileModel(SecteUserManager manager)
        {
            _userManager = manager;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public PublicProfile Profile { get; set; }
        public class InputModel
        {
            [Display(Name = "Pseudonyme")]
            [Required]
            [StringLength(30, MinimumLength = 1, ErrorMessage = "Le pseudonyme doit avoir au minimum {1} caractères et au maximum {2}")]
            public string DisplayName { get; set; }
        }
        public async Task<IActionResult> OnGet()
        {
            var profile = await _userManager.GetPublicProfileByUserAsync(User);
            if (profile is null)
            {
                return NotFound();
            }
            Profile = profile;
            Input = new InputModel
            {
                DisplayName = profile.DisplayName
            };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.GetUserWithProfileAsync(User);
            if (user?.PublicProfile is null)
            {
                return NotFound();
            }

            if (user.PublicProfile.DisplayName != Input.DisplayName)
            {
                user.PublicProfile.DisplayName = Input.DisplayName;
            }

            await _userManager.UpdateAsync(user);
            return RedirectToPage();
        }
    }
}