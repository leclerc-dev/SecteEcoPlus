using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecteEcoPlus.Areas.Identity;
using SecteEcoPlus.Areas.Identity.Data;
using SecteEcoPlus.Models;

namespace SecteEcoPlus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private SecteUserManager _userManager;
        private WebsiteContext _context;
        private SignInManager<SecteUser> _signInManager;

        public const int DownvotedExperiencePenalty = 18;
#if DEBUG
        public const int DownvotesUntilRemoval = 2;
#else
        public const int DownvotesUntilRemoval = 6;
#endif
        public const int UpvoteExperienceBonus = 14;
        public const int VoterExperienceBonus = 3;

        public IdeasController(WebsiteContext context, SecteUserManager userManager, SignInManager<SecteUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost, Route("upvote/{id}")]
        public async Task<IActionResult> Upvote(int id) => await CreateVote(id, 1);
        [HttpPost, Route("downvote/{id}")]
        public async Task<IActionResult> Downvote(int id) => await CreateVote(id, -1);
       
        private async Task<IActionResult> CreateVote(int id, int direction)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return Unauthorized();
            }

            var profile = await _userManager.GetPublicProfileByUserAsync(User);
            if (profile is null)
            {
                return BadRequest();
            }
            var idea = await _context.ProductIdeas
                .Select(i => new
                {
                    i.Id,
                    Author = profile.PublicProfileId == i.Author.PublicProfileId ? new PublicProfile() : i.Author, // dummy profile to not self vote 
                    ExisitingVote = i.Votes.FirstOrDefault(v => v.IssuerPublicProfileId == profile.PublicProfileId),
                    VoteCount = i.Votes.Sum(v => v.Direction)
                })
                .FirstOrDefaultAsync(i => i.Id == id);
            if (idea is null)
            {
                return NotFound();
            }

            var diff = idea.ExisitingVote != null && idea.ExisitingVote.Direction != direction ? direction * 2 : idea.ExisitingVote != null ? -direction : direction;
            var isFromDownVote = idea.ExisitingVote?.Direction == -1 || idea.ExisitingVote is null && direction == -1;
            _context.Attach(idea.Author);

            if (idea.VoteCount + diff <= -DownvotesUntilRemoval)
            {
                // bad post è_é
                idea.Author.RemoveExperience(DownvotedExperiencePenalty);
                _context.ProductIdeas.Remove(await _context.ProductIdeas.FindAsync(idea.Id));
                goto confirm; // goto bad? no u
            }

            if (idea.ExisitingVote != null)
            {
                if (!TryReplaceExistingVote(direction, idea.ExisitingVote))
                {
                    profile.ExperienceDebt += VoterExperienceBonus;
                }
            }
            else
            {
                await InsertNewVoteAsync(direction, profile, idea.Id);
                profile.AddExperience(VoterExperienceBonus);
            }
            ProcessExperience(diff, idea.Author, isFromDownVote);

            confirm:
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdea(int id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return Unauthorized();
            }

            var idea = await _context.ProductIdeas.FindAsync(id);
            if (idea is null) return NotFound();

            if (idea.AuthorPublicProfileId != await _userManager.GetPublicProfileIdByUserAsync(User))
            {
                return Unauthorized();
            }

            _context.ProductIdeas.Remove(idea);
            await _context.SaveChangesAsync();
            return Ok();
        }
        private static void ProcessExperience(int diff, PublicProfile author, bool isFromDownVote = false)
        {
            if (diff == 0) return;
            var sign = Math.Clamp(diff, -1, 1);
            var downvoteXp = diff * UpvoteExperienceBonus / 4;
            var xp = UpvoteExperienceBonus * sign;
            if (isFromDownVote && diff <= 1) xp = downvoteXp;
            if (diff > 1 || diff < -1) // big vote (upvote -> downvote)
            {
                xp += (downvoteXp / diff) * sign;
            }
            author.AddExperience(xp);
        }
        /// <summary>
        /// Replaces the existing vote, or deletes it if direction == 0
        /// </summary>
        /// <param name="direction">The vote direction</param>
        /// <param name="existingVote">The existing vote</param>
        /// <returns>Whether ot not the vote has been replaced or deleted</returns>
        private bool TryReplaceExistingVote(int direction, ProductIdeaVote existingVote)
        {
            if (existingVote.Direction == direction)
            {
                _context.ProductIdeaVotes.Remove(existingVote);
                return false;
            }
            else
            {
                _context.Attach(existingVote);
                existingVote.Direction = direction;
                return true;
            }
        }

        private async Task InsertNewVoteAsync(int direction, PublicProfile profile, int ideaId)
        {
            var vote = new ProductIdeaVote
            {
                Direction = direction,
                Issuer = profile,
                ProductIdeaId = ideaId
            };
            await _context.ProductIdeaVotes.AddAsync(vote);
        }
    }
}