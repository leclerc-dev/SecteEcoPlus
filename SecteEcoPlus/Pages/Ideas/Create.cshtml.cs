using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecteEcoPlus.Areas.Identity;
using SecteEcoPlus.Models;

namespace SecteEcoPlus.Pages.Ideas
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private SecteUserManager _userManager;
        private WebsiteContext _context;

        public CreateModel(SecteUserManager manager, WebsiteContext context)
        {
            _userManager = manager;
            _context = context;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "L'idée est manquante :/")]
            [Display(Name = "Idée de produit", Description = "Décrit le produit que vous voulez concevoir")]
            [StringLength(500, MinimumLength = 4, ErrorMessage = "L'{0} doit être moins longue que {1} caractères et pas trop petite (min {2}) !")]
            public string Content { get; set; }
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var profile = await _userManager.GetPublicProfileByUserAsync(User);
            if (profile is null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var idea = new ProductIdea
            {
                Author = profile,
                Content = Input.Content,
                PublishDate = DateTime.Now
            };
            await _context.ProductIdeas.AddAsync(idea);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index", new { refresh = true });
        }
    }
}