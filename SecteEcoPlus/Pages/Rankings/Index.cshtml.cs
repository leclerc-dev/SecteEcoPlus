using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecteEcoPlus.Areas.Identity.Data;
using SecteEcoPlus.Models;
using SecteEcoPlus.ViewModels;

namespace SecteEcoPlus.Pages.Rankings
{
    public class IndexModel : PageModel
    {
        private WebsiteContext _context;
        public IReadOnlyList<ProfileRanking> Rankings { get; set; }
        public IndexModel(WebsiteContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGet()
        {
            var profiles = await _context.PublicProfiles
                .AsNoTracking()
                .OrderByDescending(p => p.Experience)
                .Select(p => new ProfileRanking
                {
                    DisplayName = p.DisplayName,
                    Experience = p.Experience,
                    Id = p.PublicProfileId
                })
                .Take(10)
                .ToArrayAsync();
            for (int i = 0; i < profiles.Length; i++)
            {
                profiles[i].Rank = i + 1;
            }
            Rankings = profiles;
            return Page();
        }
    }
}