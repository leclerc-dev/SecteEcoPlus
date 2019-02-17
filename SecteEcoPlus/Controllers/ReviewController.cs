using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecteEcoPlus.Areas.Identity;
using SecteEcoPlus.Areas.Identity.Data;
using SecteEcoPlus.Models;
using SecteEcoPlus.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SecteEcoPlus.Controllers
{
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly WebsiteContext _context;
        private readonly SecteUserManager _userManager;
        private readonly SignInManager<SecteUser> _signInManager;
        private readonly Random _random;

        private const int ReviewSubmitExperienceReward = 13;
        private const int SubsequentReviewModifier = 2;
        public ReviewController(WebsiteContext context, SecteUserManager userManager, SignInManager<SecteUser> signInManager, Random random)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _random = random;
        }
        [HttpPost, Route("api/submit-review")]
        public async Task<IActionResult> Submit([FromForm] MessageRequestViewModel messageRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            PublicProfile profile = null;
            if (!messageRequest.IsAnonymous && _signInManager.IsSignedIn(User))
            {
                profile = await _userManager.GetPublicProfileByUserAsync(User);
            }
            var model = new Message
            {
                Author = profile,
                Content = messageRequest.Content,
                PublishDate = DateTime.Now
            };
            if (profile != null)
            {
                var baseValue = ReviewSubmitExperienceReward;
                var recentMessages = await GetRecentMessagesCountAsync(profile) * SubsequentReviewModifier;
                baseValue = Math.Max(baseValue - recentMessages, 0);
                if (baseValue > 0)
                {
                    var realXpGained = profile.AddExperience(baseValue);
                    if (realXpGained > 0)
                    {
                        ViewBag.HasExperience = true;
                        ViewBag.GainedExperience = baseValue;
                    }
                }
            }
            await _context.WebsiteReviews.AddAsync(model);
            await _context.SaveChangesAsync();
            ViewBag.IsRecent = true;
            return PartialView("_ReviewPartial", model);
        }

        private async Task<int> GetRecentMessagesCountAsync(PublicProfile profile)
        {
            return await _context.WebsiteReviews.Where(m =>
                m.AuthorId == profile.PublicProfileId && DateTime.Now.Subtract(m.PublishDate) < TimeSpan.FromHours(3)).CountAsync();
        }

        [HttpGet, Route("api/delete-review/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return Unauthorized();
            }
            var user = await _userManager.GetPublicProfileByUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }
            var message = await _context.WebsiteReviews.FindAsync(id);
            if (message is null)
            {
                return NotFound();
            }

            if (message.AuthorId != user.PublicProfileId)
            {
                return Unauthorized();
            }
            // alright everything is fine
            _context.WebsiteReviews.Remove(message);
            user.ExperienceDebt += Math.Max(ReviewSubmitExperienceReward - await GetRecentMessagesCountAsync(user) * SubsequentReviewModifier + SubsequentReviewModifier, 0);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}