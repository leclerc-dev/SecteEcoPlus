using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecteEcoPlus.Areas.Identity;
using SecteEcoPlus.Models;
using SecteEcoPlus.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;
using SecteEcoPlus.Areas.Identity.Data;

namespace SecteEcoPlus.Pages.Ideas
{
    public class IndexModel : PageModel
    {
        public class Love : DynamicObject
        {
            private static readonly string[] Messages = {
                "I love you, my {0}",
                "I love {0}",
                "{0} is my favorite",
                "{0} is the best",
                "I love {0} so much!!!",
                "{0} is my bae",
                "{0} best thing ever",
                "OwO i luv {0} ;3"
            };
            private static readonly Random Random = new Random();
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = string.Format(Messages[Random.Next(0, Messages.Length)].Split('_').Aggregate((a, b) => $"{a} {b}"), binder.Name);
                return true;
            }
        }
        private readonly WebsiteContext _context;
        private readonly SecteUserManager _manager;
        private readonly SignInManager<SecteUser> _signInManager;
        public IEnumerable<ProductIdeaItemViewModel> Ideas { get; set; }

        public IndexModel(WebsiteContext context, SecteUserManager manager, SignInManager<SecteUser> signInManager)
        {
            _context = context;
            _manager = manager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var ideas = await GetFeaturedIdeas();
            Ideas = ideas;
            return Page();
        }

        private async Task<IEnumerable<ProductIdeaItemViewModel>> GetFeaturedIdeas()
        {
            var profileId = !_signInManager.IsSignedIn(User) ? null : await _manager.GetPublicProfileIdByUserAsync(User);
            var hasUser = profileId.HasValue;
            var ideas = await _context
                .ProductIdeas
                .AsNoTracking()
                .Select(i => new
                {
                    VM = new ProductIdeaItemViewModel
                    {
                        Id = i.Id,
                        Content = i.Content,
                        Author = new PublicProfileViewModel
                        {
                            DisplayName = i.Author.DisplayName,
                            Id = i.AuthorPublicProfileId
                        },
                        PublishDate = i.PublishDate,
                        TotalVotes = i.Votes.Any() ? i.Votes.Sum(v => v.Direction) : 0, // prevent NULL values
                        IsFromUser = i.AuthorPublicProfileId == profileId
                    },
                    TimeBatch = Math.Floor(EF.Functions.DateDiffHour(i.PublishDate, DateTime.Now) / 25d),
                    CurrentUserVote = i.Votes.Where(v => hasUser && v.IssuerPublicProfileId == profileId).Select(v => v.Direction).ToList()
                })
                .OrderBy(i => i.TimeBatch)
                .ThenByDescending(i => i.VM.TotalVotes)
                .ThenByDescending(i => i.VM.PublishDate).ToListAsync();

            if (!hasUser) return ideas.Select(i => i.VM).ToList();
            foreach (var idea in ideas)
            {
                idea.VM.VoteDirection = idea.CurrentUserVote.FirstOrDefault();
            }

            return ideas.Select(i => i.VM).ToList();
        }
    }
}