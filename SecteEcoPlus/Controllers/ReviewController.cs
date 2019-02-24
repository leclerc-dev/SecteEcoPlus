using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecteEcoPlus.Areas.Identity;
using SecteEcoPlus.Areas.Identity.Data;
using SecteEcoPlus.Models;
using SecteEcoPlus.ViewModels;
using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace SecteEcoPlus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        //public class Stupid : DynamicObject
        //{
        //    public override bool TryGetMember(GetMemberBinder binder, out object result)
        //    {
        //        var b = new StringBuilder();
        //        for (int i = 0; i < binder.Name.Length; i++)
        //        {
        //            var @char = binder.Name[i];
        //            if (char.IsUpper(@char) && i != 0)
        //            {
        //                b.Append(' ');
        //            }
        //            b.Append(@char);
        //        }
        //        result = b.ToString();
        //        return true;
        //    }
        //} idk
        private readonly WebsiteContext _context;
        private readonly SecteUserManager _userManager;
        private readonly SignInManager<SecteUser> _signInManager;
        private readonly Random _random;
        private readonly IMemoryCache _cache;

        private const int ReviewSubmitExperienceReward = 4;
        private const int SubsequentReviewModifier = 4;
        public ReviewController(WebsiteContext context, SecteUserManager userManager, SignInManager<SecteUser> signInManager, Random random, IMemoryCache cache)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _random = random;
            _cache = cache;
        }
        [HttpPost]
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
            _cache.Remove(Cache.LastReviews); // refresh the cache
            ViewBag.IsRecent = true;
            return PartialView("_ReviewPartial", model);
        }

        private async Task<int> GetRecentMessagesCountAsync(PublicProfile profile, DateTime? relativeTo = null)
        {
            relativeTo = relativeTo ?? DateTime.Now;
            return await _context.WebsiteReviews.Where(m =>
                m.AuthorId == profile.PublicProfileId && relativeTo.Value.Subtract(m.PublishDate) < TimeSpan.FromHours(3)).CountAsync();
        }

        [HttpDelete("{id}")]
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
            user.ExperienceDebt += Math.Max(ReviewSubmitExperienceReward - await GetRecentMessagesCountAsync(user, message.PublishDate) * SubsequentReviewModifier + SubsequentReviewModifier, 0);
            _cache.Remove(Cache.LastReviews);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}